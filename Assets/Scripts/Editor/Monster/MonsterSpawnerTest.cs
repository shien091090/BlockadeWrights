using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Monster
{
    public class MonsterSpawnerTest
    {
        private MonsterSpawner monsterSpawner;
        private Action onSpawnMonster;

        [SetUp]
        public void Setup()
        {
            onSpawnMonster = Substitute.For<Action>();
            monsterSpawner = new MonsterSpawner();
            monsterSpawner.OnSpawnMonster += onSpawnMonster;
        }

        [Test]
        //產怪一次
        public void spawn_monster_one_time()
        {
            AttackWave wave1 = new AttackWave(3, 0);
            monsterSpawner.SetAttackWave(wave1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldAllWavesSpawnFinished(false);
            ShouldTriggerSpawnEvent(1);
        }

        [Test]
        //產怪至上限
        public void spawn_monster_to_max()
        {
            AttackWave wave1 = new AttackWave(3, 0);
            monsterSpawner.SetAttackWave(wave1);
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
            monsterSpawner.SetAttackWave(wave1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldAllWavesSpawnFinished(true);
            ShouldTriggerSpawnEvent(3);
        }

        [Test]
        //產怪後等待一段時間，再產怪
        public void spawn_monster_wait_time()
        {
            AttackWave wave1 = new AttackWave(3, 1);
            monsterSpawner.SetAttackWave(wave1);

            monsterSpawner.CheckUpdateSpawn(0.5f);
            ShouldTriggerSpawnEvent(0);

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
            monsterSpawner.SetAttackWave(wave1, wave2);

            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            ShouldWaveCanSpawnNext(wave1, false);

            monsterSpawner.CheckUpdateSpawn(1);
            CurrentWaveIndexShouldBe(0);

            monsterSpawner.CheckUpdateSpawn(1);
            CurrentWaveIndexShouldBe(1);

            ShouldTriggerSpawnEvent(3);
            ShouldAllWavesSpawnFinished(false);
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

        //第一波產怪結束, 沒有第二波
        //第一波產怪中, 第一波時間到開始第二波產怪

        private void ShouldTriggerSpawnEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                onSpawnMonster.DidNotReceive().Invoke();
            else
                onSpawnMonster.Received(triggerTimes).Invoke();
        }
    }
}