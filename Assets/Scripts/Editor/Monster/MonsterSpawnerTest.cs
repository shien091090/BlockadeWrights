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
            monsterSpawner.SetAttackWave(3, 0);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldCanSpawnNext(true);
            ShouldTriggerSpawnEvent(1);
        }

        [Test]
        //產怪至上限
        public void spawn_monster_to_max()
        {
            monsterSpawner.SetAttackWave(3, 0);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldCanSpawnNext(false);
            ShouldTriggerSpawnEvent(3);
        }

        [Test]
        //產怪超過上限
        public void spawn_monster_over_max()
        {
            monsterSpawner.SetAttackWave(3, 0);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);
            monsterSpawner.CheckUpdateSpawn(1);

            ShouldCanSpawnNext(false);
            ShouldTriggerSpawnEvent(3);
        }

        [Test]
        //產怪後等待一段時間，再產怪
        public void spawn_monster_wait_time()
        {
            monsterSpawner.SetAttackWave(3, 1);

            monsterSpawner.CheckUpdateSpawn(0.5f);
            ShouldTriggerSpawnEvent(0);

            monsterSpawner.CheckUpdateSpawn(0.5f);
            ShouldTriggerSpawnEvent(1);
            ShouldCanSpawnNext(true);
        }

        //第一波時間到, 開始第二波產怪
        //第一波產怪中, 第一波時間到開始第二波產怪

        private void ShouldTriggerSpawnEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                onSpawnMonster.DidNotReceive().Invoke();
            else
                onSpawnMonster.Received(triggerTimes).Invoke();
        }

        private void ShouldCanSpawnNext(bool expectedCanSpawnNext)
        {
            Assert.AreEqual(expectedCanSpawnNext, monsterSpawner.CanSpawnNext);
        }
    }
}