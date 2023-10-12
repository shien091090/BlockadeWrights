using System;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.AutoAttack
{
    public class AutoAttackModelTest
    {
        private const int DEFAULT_ATTACK_RANGE = 10;
        private const int DEFAULT_ATTACK_FREQUENCY = 1;
        private const int DEFAULT_ATTACK_POWER = 1;

        private AutoAttackModel autoAttackModel;
        private IBuildingAttackView buildingAttackView;

        [SetUp]
        public void Setup()
        {
            buildingAttackView = Substitute.For<IBuildingAttackView>();
        }

        [Test]
        //未進入攻擊範圍, 不攻擊
        public void not_in_attack_range()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            AttackTargetCountShouldBe(0);
        }

        [Test]
        //進入攻擊範圍, 但未設定攻擊頻率, 不攻擊
        public void in_attack_range_but_not_set_attack_frequency()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, 0, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTargetProvider attackTargetProvider = CreateAttackTargetProvider(CreateAttackTarget());
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));

            autoAttackModel.UpdateAttackTimer(1);

            ShouldNotLaunchAttack();
        }

        [Test]
        //進入攻擊範圍, 攻擊
        public void in_attack_range_then_attack()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget = CreateAttackTarget();
            IAttackTargetProvider attackTargetProvider = CreateAttackTargetProvider(attackTarget);
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));

            autoAttackModel.UpdateAttackTimer(0.5f);

            ShouldNotLaunchAttack();

            autoAttackModel.UpdateAttackTimer(0.5f);

            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER);
        }

        [Test]
        //進入攻擊範圍, 攻擊, CD時間結束後對象離開攻擊範圍, 不攻擊
        public void in_attack_range_then_attack_then_target_leave_attack_range()
        {
            autoAttackModel = new AutoAttackModel(4, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget = CreateAttackTarget();
            IAttackTargetProvider attackTargetProvider = CreateAttackTargetProvider(attackTarget);
            GivenAttackTargetPos(attackTarget, new Vector2(2, 3));

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER, 1);

            GivenAttackTargetPos(attackTarget, new Vector2(3, 4));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(0);
            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER, 1);
        }

        [Test]
        //進入攻擊範圍, 攻擊, CD時間結束後對象尚未離開攻擊範圍, 再攻擊一次
        public void in_attack_range_then_attack_then_target_still_in_attack_range()
        {
            autoAttackModel = new AutoAttackModel(5, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget = CreateAttackTarget();
            IAttackTargetProvider attackTargetProvider = CreateAttackTargetProvider(attackTarget);
            GivenAttackTargetPos(attackTarget, new Vector2(2, 3));

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER, 1);

            GivenAttackTargetPos(attackTarget, new Vector2(3, 4));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER, 2);
        }

        [Test]
        //進入攻擊範圍, 攻擊, CD時間結束前對象離開後又進入攻擊範圍, 再攻擊一次
        public void in_attack_range_then_attack_then_target_leave_attack_range_then_enter_attack_range()
        {
            autoAttackModel = new AutoAttackModel(4, 1, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget = CreateAttackTarget();
            IAttackTargetProvider attackTargetProvider = CreateAttackTargetProvider(attackTarget);
            GivenAttackTargetPos(attackTarget, new Vector2(2, 3));

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));
            autoAttackModel.UpdateAttackTimer(1);

            AttackTargetCountShouldBe(1);
            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER, 1);

            GivenAttackTargetPos(attackTarget, new Vector2(3, 4));
            autoAttackModel.UpdateAttackTimer(0.5f);

            AttackTargetCountShouldBe(1);

            GivenAttackTargetPos(attackTarget, new Vector2(2, 3));
            autoAttackModel.UpdateAttackTimer(0.5f);

            AttackTargetCountShouldBe(1);
            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER, 2);
        }

        [Test]
        //同一個目標重複觸發攻擊範圍多次, 驗證攻擊目標列表中是否只有一個目標
        public void same_target_trigger_attack_range_multiple_times()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget = CreateAttackTarget();
            IAttackTargetProvider attackTargetProvider = CreateAttackTargetProvider(attackTarget);

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));

            AttackTargetCountShouldBe(1);
        }

        [Test]
        //多個目標進入攻擊範圍後離開, 只剩一個攻擊目標
        public void multiple_targets_enter_and_leave_attack_range()
        {
            autoAttackModel = new AutoAttackModel(3, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget1 = CreateAttackTarget("1", new Vector2(0, 0));
            IAttackTargetProvider attackTargetProvider1 = CreateAttackTargetProvider(attackTarget1);
            IAttackTarget attackTarget2 = CreateAttackTarget("2", new Vector2(0, 0));
            IAttackTargetProvider attackTargetProvider2 = CreateAttackTargetProvider(attackTarget2);
            IAttackTarget attackTarget3 = CreateAttackTarget("3", new Vector2(0, 0));
            IAttackTargetProvider attackTargetProvider3 = CreateAttackTargetProvider(attackTarget3);
            IAttackTarget attackTarget4 = CreateAttackTarget("4", new Vector2(0, 0));
            IAttackTargetProvider attackTargetProvider4 = CreateAttackTargetProvider(attackTarget4);

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider1));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider2));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider3));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider4));

            AttackTargetCountShouldBe(4);

            GivenAttackTargetPos(attackTarget1, new Vector2(5, 5));
            GivenAttackTargetPos(attackTarget3, new Vector2(5, 5));
            GivenAttackTargetPos(attackTarget4, new Vector2(7, 6));

            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
        }

        [Test]
        //攻擊後, 對象死亡, 不再攻擊
        public void attack_then_target_dead()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget = CreateAttackTarget();
            IAttackTargetProvider attackTargetProvider = CreateAttackTargetProvider(attackTarget);

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));

            AttackTargetCountShouldBe(1);

            GivenAttackTargetState(attackTarget, EntityState.Dead);
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER, 1);
            AttackTargetCountShouldBe(0);

            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER, 1);
        }

        [Test]
        //攻擊時對象已即將死亡, 下次攻擊將會忽略此對象
        public void attack_when_target_is_about_to_dead()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget1 = CreateAttackTarget("1", new Vector2(2, 2));
            IAttackTargetProvider attackTargetProvider1 = CreateAttackTargetProvider(attackTarget1);
            IAttackTarget attackTarget2 = CreateAttackTarget("2", new Vector2(2, 1));
            IAttackTargetProvider attackTargetProvider2 = CreateAttackTargetProvider(attackTarget2);

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider1));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider2));

            AttackTargetCountShouldBe(2);

            GivenAttackTargetState(attackTarget2, EntityState.PreDie);

            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
            ShouldPreDamageAttackTarget(attackTarget1, 0);
            ShouldPreDamageAttackTarget(attackTarget2, 1);
            ShouldLaunchAttack(attackTarget2.Id, DEFAULT_ATTACK_POWER, 1);

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider1));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider2));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
            ShouldPreDamageAttackTarget(attackTarget1, 1);
            ShouldPreDamageAttackTarget(attackTarget2, 1);
            ShouldLaunchAttack(attackTarget1.Id, DEFAULT_ATTACK_POWER, 1);
        }

        [Test]
        //攻擊範圍中有多個目標, 攻擊距離最近的對象
        public void multiple_targets_in_attack_range()
        {
            autoAttackModel = new AutoAttackModel(10, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget1 = CreateAttackTarget("1", new Vector2(3, 3));
            IAttackTargetProvider attackTargetProvider1 = CreateAttackTargetProvider(attackTarget1);
            IAttackTarget attackTarget2 = CreateAttackTarget("2", new Vector2(2, 2));
            IAttackTargetProvider attackTargetProvider2 = CreateAttackTargetProvider(attackTarget2);
            IAttackTarget attackTarget3 = CreateAttackTarget("3", new Vector2(4, 4));
            IAttackTargetProvider attackTargetProvider3 = CreateAttackTargetProvider(attackTarget3);
            IAttackTarget attackTarget4 = CreateAttackTarget("4", new Vector2(5, 5));
            IAttackTargetProvider attackTargetProvider4 = CreateAttackTargetProvider(attackTarget4);

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider1));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider2));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider3));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider4));

            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            ShouldLaunchAttack(attackTarget1.Id, DEFAULT_ATTACK_POWER, 0);
            ShouldLaunchAttack(attackTarget2.Id, DEFAULT_ATTACK_POWER, 1);
            ShouldLaunchAttack(attackTarget3.Id, DEFAULT_ATTACK_POWER, 0);
            ShouldLaunchAttack(attackTarget4.Id, DEFAULT_ATTACK_POWER, 0);
        }

        [Test]
        //攻擊範圍中最近目標, CD時間結束後有其他更近的目標, 攻擊距離最近的對象
        public void attack_target_then_other_target_in_range()
        {
            autoAttackModel = new AutoAttackModel(10, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget1 = CreateAttackTarget("1", new Vector2(4, 4));
            IAttackTargetProvider attackTargetProvider1 = CreateAttackTargetProvider(attackTarget1);
            IAttackTarget attackTarget2 = CreateAttackTarget("2", new Vector2(3, 3));
            IAttackTargetProvider attackTargetProvider2 = CreateAttackTargetProvider(attackTarget2);
            IAttackTarget attackTarget3 = CreateAttackTarget("3", new Vector2(2, 3));
            IAttackTargetProvider attackTargetProvider3 = CreateAttackTargetProvider(attackTarget3);
            IAttackTarget attackTarget4 = CreateAttackTarget("4", new Vector2(5, 5));
            IAttackTargetProvider attackTargetProvider4 = CreateAttackTargetProvider(attackTarget4);

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider1));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider2));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider3));
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider4));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            ShouldLaunchAttack(attackTarget1.Id, DEFAULT_ATTACK_POWER, 0);
            ShouldLaunchAttack(attackTarget2.Id, DEFAULT_ATTACK_POWER, 0);
            ShouldLaunchAttack(attackTarget3.Id, DEFAULT_ATTACK_POWER, 1);
            ShouldLaunchAttack(attackTarget4.Id, DEFAULT_ATTACK_POWER, 0);

            IAttackTarget attackTarget5 = CreateAttackTarget("5", new Vector2(1, 1));
            IAttackTargetProvider attackTargetProvider5 = CreateAttackTargetProvider(attackTarget5);
            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider5));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            ShouldLaunchAttack(attackTarget1.Id, DEFAULT_ATTACK_POWER, 0);
            ShouldLaunchAttack(attackTarget2.Id, DEFAULT_ATTACK_POWER, 0);
            ShouldLaunchAttack(attackTarget3.Id, DEFAULT_ATTACK_POWER, 1);
            ShouldLaunchAttack(attackTarget4.Id, DEFAULT_ATTACK_POWER, 0);
            ShouldLaunchAttack(attackTarget5.Id, DEFAULT_ATTACK_POWER, 1);
        }

        [Test]
        //停止自動攻擊
        public void stop_auto_attack()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, 3, Vector2.zero, DEFAULT_ATTACK_POWER, buildingAttackView);

            IAttackTarget attackTarget = CreateAttackTarget();
            IAttackTargetProvider attackTargetProvider = CreateAttackTargetProvider(attackTarget);

            autoAttackModel.ColliderTriggerStay(CreateTriggerCollider(attackTargetProvider));
            autoAttackModel.UpdateAttackTimer(2.8f);
            autoAttackModel.SetStopState(true);

            ShouldNotLaunchAttack();

            autoAttackModel.UpdateAttackTimer(0.2f);

            ShouldNotLaunchAttack();

            autoAttackModel.SetStopState(false);
            autoAttackModel.UpdateAttackTimer(0.2f);

            ShouldLaunchAttack(attackTarget.Id, DEFAULT_ATTACK_POWER);
        }

        private void GivenAttackTargetState(IAttackTarget attackTarget, EntityState targetState)
        {
            attackTarget.GetEntityState.Returns(targetState);
        }

        private void GivenAttackTargetId(IAttackTarget attackTarget, string id)
        {
            attackTarget.Id.Returns(id);
        }

        private void GivenAttackTargetPos(IAttackTarget attackTarget, Vector2 targetPos)
        {
            ITransform transform = Substitute.For<ITransform>();
            transform.Position.Returns(targetPos);
            attackTarget.GetTransform.Returns(transform);
        }

        private void ShouldPreDamageAttackTarget(IAttackTarget attackTarget, int callTimes)
        {
            if (callTimes == 0)
                attackTarget.DidNotReceive().PreDamage(Arg.Any<float>());
            else
                attackTarget.Received(callTimes).PreDamage(Arg.Any<float>());
        }

        private void ShouldNotLaunchAttack()
        {
            buildingAttackView.DidNotReceive().LaunchAttack(Arg.Any<IAttackTarget>(), Arg.Any<float>());
        }

        private void ShouldLaunchAttack(string targetId, int attackPower, int callTimes = 1)
        {
            buildingAttackView.Received(callTimes).LaunchAttack(Arg.Is<IAttackTarget>(target => target.Id == targetId), attackPower);
        }

        private void AttackTargetCountShouldBe(int expectedCount)
        {
            Assert.AreEqual(expectedCount, autoAttackModel.AttackTargets.Count);
        }

        private IAttackTargetProvider CreateAttackTargetProvider(IAttackTarget attackTarget)
        {
            IAttackTargetProvider attackTargetProvider = Substitute.For<IAttackTargetProvider>();
            attackTargetProvider.GetAttackTarget.Returns(attackTarget);
            return attackTargetProvider;
        }

        private ITriggerCollider CreateTriggerCollider<T>(T attackTarget)
        {
            ITriggerCollider triggerCollider = Substitute.For<ITriggerCollider>();
            triggerCollider.GetComponent<T>().Returns(attackTarget);
            return triggerCollider;
        }

        private IAttackTarget CreateAttackTarget(string id = null, Vector2 pos = default)
        {
            IAttackTarget attackTarget = Substitute.For<IAttackTarget>();
            GivenAttackTargetId(attackTarget, id);
            GivenAttackTargetPos(attackTarget, pos);
            GivenAttackTargetState(attackTarget, EntityState.Normal);

            return attackTarget;
        }
    }
}