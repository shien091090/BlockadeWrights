using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Monster
{
    public class MonsterSpawnerTest
    {
        private MonsterSpawner monsterSpawner;
        private Action spawnMonsterEvent;
        private Action startNextWaveEvent;

        [SetUp]
        public void Setup()
        {
            spawnMonsterEvent = Substitute.For<Action>();
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

            Assert.AreEqual(expectedWaveHint, monsterSpawner.GetWaveHint);
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
                spawnMonsterEvent.DidNotReceive().Invoke();
            else
                spawnMonsterEvent.Received(triggerTimes).Invoke();
        }
    }
}