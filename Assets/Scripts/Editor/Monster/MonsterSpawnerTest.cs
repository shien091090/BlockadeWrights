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
            monsterSpawner.SetAttackWave(3);
            monsterSpawner.Spawn();

            ShouldCanSpawnNext(true);
            ShouldTriggerSpawnEvent(1);
        }

        [Test]
        //產怪至上限
        public void spawn_monster_to_max()
        {
            monsterSpawner.SetAttackWave(3);
            monsterSpawner.Spawn();
            monsterSpawner.Spawn();
            monsterSpawner.Spawn();

            ShouldCanSpawnNext(false);
            ShouldTriggerSpawnEvent(3);
        }

        [Test]
        //產怪超過上限
        public void spawn_monster_over_max()
        {
            monsterSpawner.SetAttackWave(3);
            monsterSpawner.Spawn();
            monsterSpawner.Spawn();
            monsterSpawner.Spawn();
            monsterSpawner.Spawn();

            ShouldCanSpawnNext(false);
            ShouldTriggerSpawnEvent(3);
        }

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