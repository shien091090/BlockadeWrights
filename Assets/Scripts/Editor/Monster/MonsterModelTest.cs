using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Monster
{
    public class MonsterModelTest
    {
        private const float DEFAULT_MOVE_SPEED = 5;
        private const float DEFAULT_HP = 100;

        private MonsterModel monsterModel;
        private ITimeManager timeManager;

        private Action onDamageFort;
        private IMonsterView monsterView;
        private ITransform transformAdapter;

        [SetUp]
        public void Setup()
        {
            timeManager = Substitute.For<ITimeManager>();
            GivenDeltaTime(1);
            monsterView = Substitute.For<IMonsterView>();
            transformAdapter = Substitute.For<ITransform>();
            GivenCurrentPosition(Vector2.zero);
            monsterView.GetTransform.Returns(transformAdapter);
            onDamageFort = Substitute.For<Action>();
        }

        [Test]
        //初始化綁定時, 執行基本設定
        public void init_bind_view()
        {
            monsterModel = new MonsterModel(
                CreateMovementPath(null),
                CreateMonsterSetting(100, 1),
                timeManager);

            monsterModel.Bind(monsterView);

            ShouldSetupHp(100);
            ShouldInitSprite();
        }

        [Test]
        //無行走路線, 不移動
        public void move_no_path()
        {
            GivenInitModel();

            monsterModel.Update();

            ShouldNoMove();
            ShouldBeArrivedGoal(false);
            ShouldBeTriggerDamageFortEvent(0);
        }

        [Test]
        //行走路徑僅起點和終點, 往終點移動
        public void move_path_start_to_end()
        {
            GivenInitModel(
                DEFAULT_HP,
                DEFAULT_MOVE_SPEED,
                new Vector2(0, 0),
                new Vector2(10, -10));

            monsterModel.Update();

            ShouldMoveRightAndDown();
            ShouldBeArrivedGoal(false);
            ShouldBeTriggerDamageFortEvent(0);
        }

        [Test]
        //行走路徑有多個點, 從中間點往下一個點移動
        public void move_path_middle_to_next()
        {
            GivenInitModel(
                DEFAULT_HP,
                DEFAULT_MOVE_SPEED,
                new Vector2(0, 0),
                new Vector2(10, -10),
                new Vector2(10, 0),
                new Vector2(50, 50));

            GivenTargetPathIndex(2);
            GivenCurrentPosition(new Vector2(10, -10));

            monsterModel.Update();

            ShouldMoveUp();
            ShouldBeArrivedGoal(false);
            ShouldBeTriggerDamageFortEvent(0);
        }

        [Test]
        [TestCase(1, 9.9f, -9.9f)]
        [TestCase(2, 10.1f, -0.1f)]
        //抵達後轉向至下一個點
        public void move_to_next_point_when_arrived(int startIndex, float currentPosX, float currentPosY)
        {
            GivenInitModel(
                DEFAULT_HP,
                1.5f,
                new Vector2(0, 0),
                new Vector2(10, -10),
                new Vector2(10, 0),
                new Vector2(50, 50));

            GivenTargetPathIndex(startIndex);
            GivenCurrentPosition(new Vector2(currentPosX, currentPosY));

            monsterModel.Update();

            CurrentTargetPathIndexShouldBe(startIndex + 1);
            ShouldBeArrivedGoal(false);
            ShouldBeTriggerDamageFortEvent(0);
        }

        [Test]
        //移動至終點, 破壞主堡
        public void move_to_end_point_and_destroy_fortress()
        {
            GivenInitModel(
                DEFAULT_HP,
                DEFAULT_MOVE_SPEED,
                new Vector2(0, 0),
                new Vector2(10, -10));

            GivenCurrentPosition(new Vector2(9.9f, -9.9f));

            monsterModel.Update();

            ShouldBeArrivedGoal(true);
            ShouldBeTriggerDamageFortEvent(1);
        }

        [Test]
        //移動至終點後不再移動
        public void move_to_end_point_and_no_move()
        {
            GivenInitModel(
                DEFAULT_HP,
                DEFAULT_MOVE_SPEED,
                new Vector2(0, 0),
                new Vector2(10, -10));

            GivenTargetPathIndex(2);
            GivenCurrentPosition(new Vector2(10, -10));

            monsterModel.Update();

            ShouldNoMove();
            ShouldBeArrivedGoal(true);
            ShouldBeTriggerDamageFortEvent(0);
        }

        [Test]
        //攻擊怪物並且怪物死亡
        public void attack_monster_and_monster_dead()
        {
            GivenInitModel(10);

            monsterModel.Damage(11);

            MonsterStateShouldBe(EntityState.Dead);
            ShouldSetActive(false);
        }

        [Test]
        //此次攻擊後怪物會死亡, 驗證怪物狀態為PreDie
        public void attack_monster_and_monster_will_dead()
        {
            GivenInitModel(10);

            monsterModel.PreDamage(11);

            MonsterStateShouldBe(EntityState.PreDie);
            ShouldNotCallSetActive();
        }

        [Test]
        //怪物死亡後, 再PreDamage仍為死亡狀態
        public void attack_monster_after_dead()
        {
            GivenInitModel(10);

            monsterModel.PreDamage(11);
            MonsterStateShouldBe(EntityState.PreDie);

            monsterModel.Damage(11);
            MonsterStateShouldBe(EntityState.Dead);

            monsterModel.PreDamage(11);
            MonsterStateShouldBe(EntityState.Dead);

            ShouldSetActive(false);
        }

        private void GivenDeltaTime(int deltaTime)
        {
            timeManager.DeltaTime.Returns(deltaTime);
        }

        private void GivenCurrentPosition(Vector2 position)
        {
            transformAdapter.Position.Returns(position);
        }

        private void GivenTargetPathIndex(int index)
        {
            monsterModel.SetTargetPathIndex(index);
        }

        private void GivenInitModel(float hp = 100, float moveSpeed = 5, params Vector2[] pathPoints)
        {
            IMonsterSetting monsterSetting = CreateMonsterSetting(hp, moveSpeed);
            MonsterMovementPath path = CreateMovementPath(pathPoints);

            monsterModel = new MonsterModel(path, monsterSetting, timeManager);
            monsterModel.OnDamageFort += onDamageFort;

            monsterModel.Bind(monsterView);
        }

        private void ShouldInitSprite(int callTimes = 1)
        {
            monsterView.Received(callTimes).InitSprite(Arg.Any<Sprite>(), Arg.Any<Sprite>());
        }

        private void ShouldSetupHp(int expectedHp, int callTimes = 1)
        {
            monsterView.Received(callTimes).SetupHp(Arg.Is<HealthPointModel>(hpModel => hpModel.CurrentHp == expectedHp));
        }

        private void ShouldNotCallSetActive()
        {
            monsterView.DidNotReceive().SetActive(Arg.Any<bool>());
        }

        private void ShouldSetActive(bool expectedActive)
        {
            bool argument = (bool)monsterView.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "SetActive").GetArguments()[0];
            Assert.AreEqual(expectedActive, argument);
        }

        private void MonsterStateShouldBe(EntityState expectedState)
        {
            Assert.AreEqual(expectedState, monsterModel.GetEntityState);
        }

        private void ShouldBeTriggerDamageFortEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                onDamageFort.DidNotReceive().Invoke();
            else
                onDamageFort.Received(triggerTimes).Invoke();
        }

        private void ShouldBeArrivedGoal(bool expectedArrived)
        {
            Assert.AreEqual(expectedArrived, monsterModel.IsArrivedGoal);
        }

        private void CurrentTargetPathIndexShouldBe(int expectedIndex)
        {
            Assert.AreEqual(expectedIndex, monsterModel.CurrentTargetPathIndex);
        }

        private void ShouldMoveUp()
        {
            Vector2 argument = (Vector2)transformAdapter.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "Translate").GetArguments()[0];
            Assert.IsTrue(argument.x == 0);
            Assert.IsTrue(argument.y > 0);
        }

        private void ShouldMoveRightAndDown()
        {
            Vector2 argument = (Vector2)transformAdapter.ReceivedCalls().Last().GetArguments()[0];
            Assert.IsTrue(argument.x > 0);
            Assert.IsTrue(argument.y < 0);
        }

        private void ShouldNoMove()
        {
            transformAdapter.DidNotReceive().Translate(Arg.Any<Vector2>());
        }

        private MonsterMovementPath CreateMovementPath(Vector2[] pathPoints)
        {
            MonsterMovementPath path;
            path = new MonsterMovementPath();
            if (pathPoints != null && pathPoints.Length > 0)
            {
                foreach (Vector2 pathPoint in pathPoints)
                {
                    path.AddPoint(pathPoint);
                }
            }

            return path;
        }

        private IMonsterSetting CreateMonsterSetting(float hp, float moveSpeed)
        {
            IMonsterSetting monsterSetting = Substitute.For<IMonsterSetting>();
            monsterSetting.GetHp.Returns(hp);
            monsterSetting.GetMoveSpeed.Returns(moveSpeed);
            return monsterSetting;
        }
    }
}