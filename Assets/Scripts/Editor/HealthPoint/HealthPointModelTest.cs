using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.HealthPoint
{
    public class HealthPointModelTest
    {
        private HealthPointModel healthPointModel;
        private IHealthPointView healthPointView;

        [Test]
        //HP上限為0
        public void max_hp_is_0()
        {
            GivenInitModel(0);
            ShouldBeInvalid(true);
        }

        [Test]
        //初始化時會先更新一次血量
        public void init_hp()
        {
            GivenInitModel(10);
            ShouldRefreshHealthPointSlider(1);
            HealthPointSliderValueShouldBe(1);
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
            ShouldRefreshHealthPointSlider(2);
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
            ShouldRefreshHealthPointSlider(3);
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

        [Test]
        [TestCase(10, 5, 0.5f)]
        [TestCase(100, 99, 0.01f)]
        [TestCase(100, 0.1f, 0.999f)]
        [TestCase(99999, 99999, 0)]
        //驗證血量變化百分比
        public void verify_hp_change_rate(float maxHp, float damageValue, float expectedHpRate)
        {
            GivenInitModel(maxHp);

            healthPointModel.Damage(damageValue);

            HealthPointSliderValueShouldBe(expectedHpRate);
        }

        [Test]
        //判斷此次攻擊是否會死亡
        public void is_dead_when_this_time_attack()
        {
            GivenInitModel(10);

            ShouldDieWhenDamage(5, false);

            healthPointModel.Damage(5);

            ShouldBeDead(false);
            ShouldDieWhenDamage(5, true);
        }

        private void GivenInitModel(float maxHp)
        {
            healthPointModel = new HealthPointModel(maxHp);
            healthPointView = Substitute.For<IHealthPointView>();
            healthPointModel.Bind(healthPointView);
        }

        private void ShouldDieWhenDamage(float damageValue, bool expectedIsDead)
        {
            Assert.AreEqual(expectedIsDead, healthPointModel.WellDieWhenDamage(damageValue));
        }

        private void HealthPointSliderValueShouldBe(float expectedHpRate)
        {
            HealthPointChangeInfo argument = (HealthPointChangeInfo)healthPointView
                .ReceivedCalls()
                .Last(x => x.GetMethodInfo().Name == "RefreshHealthPointSlider")
                .GetArguments()[0];

            Assert.AreEqual(expectedHpRate, argument.CurrentHealthPointRate);
        }

        private void ShouldRefreshHealthPointSlider(int triggerTimes)
        {
            if (triggerTimes == 0)
                healthPointView.DidNotReceive().RefreshHealthPointSlider(Arg.Any<HealthPointChangeInfo>());
            else
                healthPointView.Received(triggerTimes).RefreshHealthPointSlider(Arg.Any<HealthPointChangeInfo>());
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
            Assert.AreEqual(expectedIsInvalid, healthPointModel.IsInValid);
        }
    }
}