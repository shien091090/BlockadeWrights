using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Fortress
{
    public class FortressTest
    {
        private Action fortressDestroyEvent;
        private FortressModel fortressModel;

        [SetUp]
        public void Setup()
        {
            fortressDestroyEvent = Substitute.For<Action>();
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
            GivenInitModel(100);
            ShouldModelInvalid(false);

            fortressModel.Damage(10);

            ShouldTriggerFortressDestroyEvent(0);
        }

        [Test]
        [TestCase(100)]
        [TestCase(100.01f)]
        [TestCase(500)]
        //主堡被攻擊並且被破壞
        public void fortress_be_attacked_and_destroyed(float damageValue)
        {
            GivenInitModel(100);

            fortressModel.Damage(damageValue);

            ShouldFortressHpShouldBe(0);
            ShouldTriggerFortressDestroyEvent(1);
        }

        private void GivenInitModel(int mapHp)
        {
            fortressModel = new FortressModel(mapHp);
            fortressModel.OnFortressDestroy += fortressDestroyEvent;
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