using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.AttackWaves
{
    public class MonsterSpawnerTest
    {
        private IAttackWaveSetting attackWaveSetting;
        private MonsterSpawner monsterSpawner;
        private Action<IMonsterModel> spawnMonsterEvent;
        private Action startNextWaveEvent;

        [SetUp]
        public void Setup()
        {
            spawnMonsterEvent = Substitute.For<Action<IMonsterModel>>();
            startNextWaveEvent = Substitute.For<Action>();
            attackWaveSetting = Substitute.For<IAttackWaveSetting>();
        }

        [Test]
        //產怪一次
        public void spawn_monster_one_time()
        {
            GivenModel(new AttackWave(0, CreateMonsterOrderList(3)));

            monsterSpawner.CheckUpdateSpawn(1);

            ShouldAllWavesSpawnFinished(false);
            ShouldTriggerSpawnEvent(1);
        }

        [Test]
        //產怪至上限
        public void spawn_monster_to_max()
        {
            GivenModel(new AttackWave(0, CreateMonsterOrderList(3)));

            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldAllWavesSpawnFinished(true);
            ShouldTriggerSpawnEvent(3);
        }

        [Test]
        //產怪超過上限
        public void spawn_monster_over_max()
        {
            GivenModel(new AttackWave(0, CreateMonsterOrderList(3)));

            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldAllWavesSpawnFinished(true);
            ShouldTriggerSpawnEvent(3);
        }

        [Test]
        //產不同怪
        public void spawn_different_monster()
        {
            GivenModel(new AttackWave(1, new List<IMonsterSetting>
            {
                CreateMonsterSetting(10),
                CreateMonsterSetting(20),
                CreateMonsterSetting(30)
            }));

            monsterSpawner.CheckUpdateSpawn(1);
            ShouldTriggerSpawnEvent(10, 1);

            monsterSpawner.CheckUpdateSpawn(1);
            ShouldTriggerSpawnEvent(20, 1);

            monsterSpawner.CheckUpdateSpawn(1);
            ShouldTriggerSpawnEvent(30, 1);
        }

        [Test]
        //第一波時間尚未到, 不產怪
        public void spawn_monster_not_at_start_time()
        {
            GivenModel(new AttackWave(0, CreateMonsterOrderList(3), 10));

            monsterSpawner.CheckUpdateSpawn(0.5f);
            monsterSpawner.CheckUpdateSpawn(0.5f);
            monsterSpawner.CheckUpdateSpawn(0.5f);
            monsterSpawner.CheckUpdateSpawn(0.5f);

            ShouldTriggerSpawnEvent(0);
            ShouldTriggerStartNextWaveEvent(0);
        }

        [Test]
        //產怪後等待一段時間，再產怪
        public void spawn_monster_wait_time()
        {
            GivenModel(new AttackWave(1, CreateMonsterOrderList(3)));

            monsterSpawner.CheckUpdateSpawn(0.5f);
            ShouldTriggerSpawnEvent(0);
            ShouldTriggerStartNextWaveEvent(1);

            monsterSpawner.CheckUpdateSpawn(0.5f);
            ShouldAllWavesSpawnFinished(false);
            ShouldTriggerSpawnEvent(1);
        }

        [Test]
        //第一波時間到, 開始第二波產怪
        public void spawn_monster_next_wave()
        {
            AttackWave wave1 = new AttackWave(1, CreateMonsterOrderList(2), 0);
            AttackWave wave2 = new AttackWave(1, CreateMonsterOrderList(2), 4);
            GivenModel(
                wave1,
                wave2);

            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            ShouldWaveCanSpawnNext(wave1, false);
            ShouldTriggerStartNextWaveEvent(1);

            monsterSpawner.CheckUpdateSpawn(1);
            CurrentWaveIndexShouldBe(0);

            monsterSpawner.CheckUpdateSpawn(1);
            CurrentWaveIndexShouldBe(1);
            ShouldTriggerStartNextWaveEvent(2);

            ShouldTriggerSpawnEvent(3);
            ShouldAllWavesSpawnFinished(false);
        }

        [Test]
        //第一波和第二波同時產怪
        public void spawn_monster_same_time()
        {
            GivenModel(
                new AttackWave(1, CreateMonsterOrderList(3), 0),
                new AttackWave(1, CreateMonsterOrderList(3), 1));

            monsterSpawner.CheckUpdateSpawn(1);
            ShouldTriggerStartNextWaveEvent(1);

            monsterSpawner.CheckUpdateSpawn(1);
            ShouldTriggerStartNextWaveEvent(2);

            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldTriggerSpawnEvent(6);
            CurrentWaveIndexShouldBe(1);
            ShouldAllWavesSpawnFinished(true);
        }

        [Test]
        [TestCase(0, "0/3")]
        [TestCase(2, "1/3")]
        [TestCase(3, "2/3")]
        [TestCase(4, "3/3")]
        //驗證波次提示
        public void spawn_monster_wave_hint(int updateTimes, string expectedWaveHint)
        {
            GivenModel(
                new AttackWave(1, CreateMonsterOrderList(1), 2),
                new AttackWave(1, CreateMonsterOrderList(1), 3),
                new AttackWave(1, CreateMonsterOrderList(1), 4));

            for (int i = 0; i < updateTimes; i++)
            {
                monsterSpawner.CheckUpdateSpawn(1);
            }

            WaveHintShouldBe(expectedWaveHint);
        }

        [Test]
        //產怪時不指定路徑, 產怪起始位置為預設中心位置
        public void spawn_monster_and_no_path()
        {
            GivenModel(new AttackWave(1, CreateMonsterOrderList(1), pathPointList: null));

            monsterSpawner.CheckUpdateSpawn(1);

            SpawnMonsterStartPosShouldBe(Vector2.zero);
        }

        [Test]
        //產怪時指定路徑, 產怪起始位置為路徑起始位置
        public void spawn_monster_and_have_path()
        {
            GivenModel(new AttackWave(1, CreateMonsterOrderList(1),
                pathPointList: new List<Vector2> { new Vector2(1, 1), new Vector2(2, 2) }));

            monsterSpawner.CheckUpdateSpawn(1);

            SpawnMonsterStartPosShouldBe(new Vector2(1, 1));
        }

        [Test]
        //第一波產怪時間非等待, 需有倒數計時
        public void spawn_monster_countdown()
        {
            GivenModel(new AttackWave(1, CreateMonsterOrderList(1), 0.5f));

            monsterSpawner.CheckUpdateSpawn(1);

            ShouldTriggerSpawnEvent(1);
            ShouldNeedCountDownToSpawnMonster(true);
        }
        
        [Test]
        //第一波產怪時間為立即, 不需有倒數計時
        public void spawn_monster_no_countdown()
        {
            GivenModel(new AttackWave(1, CreateMonsterOrderList(1), 0));

            monsterSpawner.CheckUpdateSpawn(0.5f);
            monsterSpawner.CheckUpdateSpawn(0.5f);

            ShouldTriggerSpawnEvent(1);
            ShouldNeedCountDownToSpawnMonster(false);
        }

        private void GivenModel(params AttackWave[] waves)
        {
            attackWaveSetting.GetAttackWaves().Returns(waves);
            monsterSpawner = new MonsterSpawner(attackWaveSetting);

            monsterSpawner.OnSpawnMonster += spawnMonsterEvent;
            monsterSpawner.OnStartNextWave += startNextWaveEvent;
        }

        private void ShouldNeedCountDownToSpawnMonster(bool expectedNeedCountDown)
        {
            Assert.AreEqual(expectedNeedCountDown, monsterSpawner.IsNeedCountDownToSpawnMonster());
        }

        private void ShouldTriggerSpawnEvent(float expectedHp, int triggerTimes = 1)
        {
            spawnMonsterEvent.Received(triggerTimes).Invoke(Arg.Is<IMonsterModel>(m => m.GetHp == expectedHp));
        }

        private void WaveHintShouldBe(string expectedWaveHint)
        {
            Assert.AreEqual(expectedWaveHint, monsterSpawner.GetWaveHint);
        }

        private void SpawnMonsterStartPosShouldBe(Vector2 expectedStartPos)
        {
            spawnMonsterEvent.Received(1).Invoke(Arg.Is<MonsterModel>(x => x.GetStartPoint == expectedStartPos));
        }

        private void CurrentWaveIndexShouldBe(int expectedWaveIndex)
        {
            Assert.AreEqual(expectedWaveIndex, monsterSpawner.GetCurrentWaveIndex);
        }

        private void ShouldWaveCanSpawnNext(AttackWave waveInfo, bool expectedCanSpawn)
        {
            Assert.AreEqual(expectedCanSpawn, waveInfo.CanSpawnNext);
        }

        private void ShouldAllWavesSpawnFinished(bool expectedAllWaveFished)
        {
            Assert.AreEqual(expectedAllWaveFished, monsterSpawner.IsAllWaveSpawnFinished);
        }

        private void ShouldTriggerStartNextWaveEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                startNextWaveEvent.DidNotReceive().Invoke();
            else
                startNextWaveEvent.Received(triggerTimes).Invoke();
        }

        private void ShouldTriggerSpawnEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                spawnMonsterEvent.DidNotReceive().Invoke(Arg.Any<MonsterModel>());
            else
                spawnMonsterEvent.Received(triggerTimes).Invoke(Arg.Any<MonsterModel>());
        }

        private List<IMonsterSetting> CreateMonsterOrderList(int spawnCount)
        {
            List<IMonsterSetting> monsterSettings = new List<IMonsterSetting>();
            for (int i = 0; i < spawnCount; i++)
            {
                IMonsterSetting monsterSetting = Substitute.For<IMonsterSetting>();
                monsterSetting.GetHp.Returns(10);
                monsterSettings.Add(monsterSetting);
            }

            return monsterSettings;
        }

        private IMonsterSetting CreateMonsterSetting(int hp)
        {
            IMonsterSetting monsterSetting = Substitute.For<IMonsterSetting>();
            monsterSetting.GetHp.Returns(hp);
            return monsterSetting;
        }
    }
}