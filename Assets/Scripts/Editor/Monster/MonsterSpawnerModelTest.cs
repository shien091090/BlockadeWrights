using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Monster
{
    public class MonsterSpawnerModelTest
    {
        private IAttackWave attackWave;
        private MonsterSpawnerModel monsterSpawnerModel;
        private Action onSpawnMonster;

        [SetUp]
        public void Setup()
        {
            attackWave = Substitute.For<IAttackWave>();
            monsterSpawnerModel = new MonsterSpawnerModel(attackWave);

            onSpawnMonster = Substitute.For<Action>();
            monsterSpawnerModel.OnSpawnMonster += onSpawnMonster;
        }

        [Test]
        //目前波次已達產怪上限
        public void current_wave_is_spawn_completed()
        {
            GivenMaxSpawnCount(5);
            GivenCurrentSpawnCount(5);

            monsterSpawnerModel.Spawn();

            ShouldCanSpawnNext(false);
            ShouldTriggerSpawnEvent(0);
        }

        private void GivenCurrentSpawnCount(int currentSpawnCount)
        {
            attackWave.GetCurrentSpawnCount.Returns(currentSpawnCount);
        }

        private void GivenMaxSpawnCount(int maxSpawnCount)
        {
            attackWave.GetMaxSpawnCount.Returns(maxSpawnCount);
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

        //目前波次未達產怪上限
    }
}