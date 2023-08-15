using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Monster
{
    public class MonsterSpawnerTest
    {
        private MonsterSpawner monsterSpawner;
        private Action<IMonsterModel> spawnMonsterEvent;
        private Action startNextWaveEvent;

        [SetUp]
        public void Setup()
        {
            spawnMonsterEvent = Substitute.For<Action<IMonsterModel>>();
            startNextWaveEvent = Substitute.For<Action>();
            monsterSpawner = new MonsterSpawner();
            monsterSpawner.OnSpawnMonster += spawnMonsterEvent;
            monsterSpawner.OnStartNextWave += startNextWaveEvent;
        }

        [Test]
        //產怪一次
        public void spawn_monster_one_time()
        {
            AttackWave wave1 = new AttackWave(3, 0);
            monsterSpawner.Init(wave1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldAllWavesSpawnFinished(false);
            ShouldTriggerSpawnEvent(1);
        }

        [Test]
        //產怪至上限
        public void spawn_monster_to_max()
        {
            AttackWave wave1 = new AttackWave(3, 0);
            monsterSpawner.Init(wave1);
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
            AttackWave wave1 = new AttackWave(3, 0);
            monsterSpawner.Init(wave1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldAllWavesSpawnFinished(true);
            ShouldTriggerSpawnEvent(3);
        }

        [Test]
        //第一波時間尚未到, 不產怪
        public void spawn_monster_not_at_start_time()
        {
            AttackWave wave1 = new AttackWave(3, 1, 10);
            monsterSpawner.Init(wave1);

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
            AttackWave wave1 = new AttackWave(3, 1);
            monsterSpawner.Init(wave1);

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
            AttackWave wave1 = new AttackWave(2, 1, 0);
            AttackWave wave2 = new AttackWave(2, 1, 4);
            monsterSpawner.Init(wave1, wave2);

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
            AttackWave wave1 = new AttackWave(3, 1, 0);
            AttackWave wave2 = new AttackWave(3, 1, 1);
            monsterSpawner.Init(wave1, wave2);

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
            AttackWave wave1 = new AttackWave(1, 1, 2);
            AttackWave wave2 = new AttackWave(1, 1, 3);
            AttackWave wave3 = new AttackWave(1, 1, 4);
            monsterSpawner.Init(wave1, wave2, wave3);

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
            AttackWave wave1 = new AttackWave(1, 1, pathPointList: null);
            monsterSpawner.Init(wave1);
            monsterSpawner.CheckUpdateSpawn(1);

            SpawnMonsterStartPosShouldBe(Vector2.zero);
        }

        [Test]
        //產怪時指定路徑, 產怪起始位置為路徑起始位置
        public void spawn_monster_and_have_path()
        {
            AttackWave wave1 = new AttackWave(1, 1, pathPointList: new List<Vector2> { new Vector2(1, 1), new Vector2(2, 2) });
            monsterSpawner.Init(wave1);
            monsterSpawner.CheckUpdateSpawn(1);

            SpawnMonsterStartPosShouldBe(new Vector2(1, 1));
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

        private void ShouldWaveCanSpawnNext(AttackWave wave1, bool expectedCanSpawn)
        {
            Assert.AreEqual(expectedCanSpawn, wave1.CanSpawnNext);
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
    }
}