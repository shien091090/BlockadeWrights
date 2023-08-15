using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Fortress
{
    public class FortressTest
    {
        private Action fortressDestroyEvent;
        private FortressModel fortressModel;
        private IMonsterSpawner monsterSpawner;
        private IMonsterModel monsterModel;

        [SetUp]
        public void Setup()
        {
            fortressDestroyEvent = Substitute.For<Action>();
            monsterSpawner = Substitute.For<IMonsterSpawner>();
            monsterModel = Substitute.For<IMonsterModel>();
        }

        [Test]
        //主堡未設置血量
        public void fortress_no_hp()
        {
            GivenInitModel(0);

            ShouldModelInvalid(true);
        }

        [Test]
        //主堡被攻擊, 但尚未被破壞
        public void fortress_be_attacked_but_not_destroyed()
        {
            GivenInitModel(3);
            ShouldModelInvalid(false);

            CallSpawnMonsterEvent();
            CallDamageFortEvent();
            CallDamageFortEvent();

            ShouldTriggerFortressDestroyEvent(0);
        }

        [Test]
        //主堡被攻擊並且被破壞
        public void fortress_be_attacked_and_destroyed()
        {
            GivenInitModel(3);

            CallSpawnMonsterEvent();
            CallDamageFortEvent();
            CallDamageFortEvent();
            CallDamageFortEvent();

            ShouldFortressHpShouldBe(0);
            ShouldTriggerFortressDestroyEvent(1);
        }

        private void GivenInitModel(int mapHp)
        {
            fortressModel = new FortressModel(mapHp, monsterSpawner);
            fortressModel.OnFortressDestroy += fortressDestroyEvent;
        }

        private void CallDamageFortEvent()
        {
            monsterModel.OnDamageFort += Raise.Event<Action>();
        }

        private void CallSpawnMonsterEvent()
        {
            monsterSpawner.OnSpawnMonster += Raise.Event<Action<IMonsterModel>>(monsterModel);
        }

        private void ShouldFortressHpShouldBe(int expectedHp)
        {
            Assert.AreEqual(expectedHp, fortressModel.CurrentHp);
        }

        private void ShouldTriggerFortressDestroyEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                fortressDestroyEvent.DidNotReceive().Invoke();
            else
                fortressDestroyEvent.Received(triggerTimes).Invoke();
        }

        private void ShouldModelInvalid(bool expectedInvalid)
        {
            Assert.AreEqual(expectedInvalid, fortressModel.IsInValid);
        }
    }
}