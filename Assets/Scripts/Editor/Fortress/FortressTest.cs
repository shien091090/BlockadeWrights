using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Fortress
{
    public class FortressTest
    {
        private Action fortressDestroyEvent;
        private FortressModel fortressModel;
        private IFortressView fortressView;
        private IHealthPointView healthPointView;
        private HealthPointModel healthPointModel;

        [SetUp]
        public void Setup()
        {
            fortressDestroyEvent = Substitute.For<Action>();
            fortressView = Substitute.For<IFortressView>();

            healthPointView = Substitute.For<IHealthPointView>();
            healthPointView.When(x => x.BindModel(Arg.Any<HealthPointModel>())).Do((bindModelFunc) =>
            {
                HealthPointModel model = bindModelFunc.Arg<HealthPointModel>();
                healthPointModel = model;
            });
            fortressView.GetHealthPointView.Returns(healthPointView);
        }

        [Test]
        //主堡未設置血量
        public void fortress_no_hp()
        {
            GivenInitModel(0);

            ShouldModelInvalid(true);
            ShouldDestroyHintActive(false);
            ShouldHpIsInvalid(true);
        }

        [Test]
        //主堡被攻擊, 但尚未被破壞
        public void fortress_be_attacked_but_not_destroyed()
        {
            GivenInitModel(3);
            ShouldModelInvalid(false);

            fortressModel.Damage();
            fortressModel.Damage();

            FortressHpShouldBe(1);
            ShouldTriggerFortressDestroyEvent(0);
        }

        [Test]
        //主堡被攻擊並且被破壞
        public void fortress_be_attacked_and_destroyed()
        {
            GivenInitModel(3);

            fortressModel.Damage();
            fortressModel.Damage();
            fortressModel.Damage();

            FortressHpShouldBe(0);
            ShouldTriggerFortressDestroyEvent(1);
        }

        private void GivenInitModel(int mapHp)
        {
            fortressModel = new FortressModel(mapHp);
            fortressModel.Bind(fortressView);
            fortressModel.OnFortressDestroy += fortressDestroyEvent;
        }

        private void ShouldHpIsInvalid(bool expectedIsInvalid)
        {
            Assert.AreEqual(expectedIsInvalid, healthPointModel.IsInValid);
        }

        private void HpShouldBe(int expectedHp)
        {
            Assert.AreEqual(expectedHp, healthPointModel.CurrentHp);
        }

        private void ShouldDestroyHintActive(bool expectedActive)
        {
            bool argument = (bool)fortressView.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "SetDestroyHintActive").GetArguments()[0];
            Assert.AreEqual(expectedActive, argument);
        }

        private void FortressHpShouldBe(int expectedHp)
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