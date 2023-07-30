using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Monster
{
    public class MonsterSpawnerModelTest
    {
        [Test]
        //目前波次已達產怪上限
        public void current_wave_is_spawn_completed()
        {
            IAttackWave attackWave = Substitute.For<IAttackWave>();
            attackWave.GetMaxSpawnCount.Returns(5);
            attackWave.GetCurrentSpawnCount.Returns(5);
            
            MonsterSpawnerModel monsterSpawnerModel = new MonsterSpawnerModel(attackWave);
            
            Action onSpawnMonster = Substitute.For<Action>();
            monsterSpawnerModel.OnSpawnMonster += onSpawnMonster;
            
            Assert.AreEqual(false, monsterSpawnerModel.CanSpawnNext);
            
            monsterSpawnerModel.Spawn();
            onSpawnMonster.DidNotReceive().Invoke();
            
        }
        
        //目前波次未達產怪上限
    }

}