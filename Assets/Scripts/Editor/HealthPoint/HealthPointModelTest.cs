using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.HealthPoint
{
    public class HealthPointModelTest
    {
        private HealthPointModel healthPointModel;
        private Action<HealthPointChangeInfo> refreshHealthPointEvent;

        [Test]
        //HP上限為0
        public void max_hp_is_0()
        {
            GivenInitModel(0);
            ShouldBeInvalid(true);
        }

        [Test]
        [TestCase(10, 5, 5)]
        [TestCase(90.5f, 0.5f, 90f)]
        [TestCase(80, 0, 80)]
        //扣血
        public void decrease_hp(float maxHp, float damageValue, float expectedRemainHp)
        {
            GivenInitModel(maxHp);

            healthPointModel.Damage(damageValue);

            ShouldBeInvalid(false);
            CurrentHpShouldBe(expectedRemainHp);
            ShouldBeDead(false);
            ShouldReceiveHpChangeEvent(1);
        }

        [Test]
        //扣血至HP歸0
        public void decrease_hp_to_0()
        {
            GivenInitModel(10);

            healthPointModel.Damage(10);

            ShouldBeInvalid(false);
            CurrentHpShouldBe(0);
            ShouldBeDead(true);
        }

        [Test]
        //扣血超出目前HP
        public void decrease_hp_over_max_hp()
        {
            GivenInitModel(10);

            healthPointModel.Damage(20);

            ShouldBeInvalid(false);
            CurrentHpShouldBe(0);
            ShouldBeDead(true);
        }

        [Test]
        //補血
        public void increase_hp()
        {
            GivenInitModel(10);

            healthPointModel.Damage(5);
            healthPointModel.Heal(3);

            CurrentHpShouldBe(8);
            ShouldBeDead(false);
            ShouldReceiveHpChangeEvent(2);
        }

        [Test]
        //補血超出HP上限
        public void increase_hp_over_max_hp()
        {
            GivenInitModel(10);

            healthPointModel.Damage(5);
            healthPointModel.Heal(10);

            CurrentHpShouldBe(10);
            ShouldBeDead(false);
        }

        private void GivenInitModel(float maxHp)
        {
            healthPointModel = new HealthPointModel(maxHp);

            refreshHealthPointEvent = Substitute.For<Action<HealthPointChangeInfo>>();
            healthPointModel.OnRefreshHealthPoint += refreshHealthPointEvent;
        }

        private void ShouldReceiveHpChangeEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                refreshHealthPointEvent.DidNotReceive().Invoke(Arg.Any<HealthPointChangeInfo>());
            else
                refreshHealthPointEvent.Received(triggerTimes).Invoke(Arg.Any<HealthPointChangeInfo>());
        }

        private void ShouldBeDead(bool expectedIsDead)
        {
            Assert.AreEqual(expectedIsDead, healthPointModel.IsDead);
        }

        private void CurrentHpShouldBe(float expectedCurrentHp)
        {
            Assert.AreEqual(expectedCurrentHp, healthPointModel.CurrentHp);
        }

        private void ShouldBeInvalid(bool expectedIsInvalid)
        {
            Assert.AreEqual(expectedIsInvalid, healthPointModel.IsValid);
        }
    }
}