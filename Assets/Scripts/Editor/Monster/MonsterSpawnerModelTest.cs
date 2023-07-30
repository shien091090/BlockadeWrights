using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Monster
{
    public class MonsterSpawnerModelTest
    {
        private MonsterSpawnerModel monsterSpawnerModel;
        private Action onSpawnMonster;

        [SetUp]
        public void Setup()
        {
            onSpawnMonster = Substitute.For<Action>();
            monsterSpawnerModel = new MonsterSpawnerModel();
            monsterSpawnerModel.OnSpawnMonster += onSpawnMonster;
        }

        [Test]
        //產怪一次
        public void spawn_monster_one_time()
        {
            monsterSpawnerModel.SetAttackWave(3);
            monsterSpawnerModel.Spawn();

            ShouldCanSpawnNext(true);
            ShouldTriggerSpawnEvent(1);
        }

        [Test]
        //產怪至上限
        public void spawn_monster_to_max()
        {
            monsterSpawnerModel.SetAttackWave(3);
            monsterSpawnerModel.Spawn();
            monsterSpawnerModel.Spawn();
            monsterSpawnerModel.Spawn();

            ShouldCanSpawnNext(false);
            ShouldTriggerSpawnEvent(3);
        }

        [Test]
        //產怪超過上限
        public void spawn_monster_over_max()
        {
            monsterSpawnerModel.SetAttackWave(3);
            monsterSpawnerModel.Spawn();
            monsterSpawnerModel.Spawn();
            monsterSpawnerModel.Spawn();
            monsterSpawnerModel.Spawn();

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
            Assert.AreEqual(expectedCanSpawnNext, monsterSpawnerModel.CanSpawnNext);
        }
    }
}