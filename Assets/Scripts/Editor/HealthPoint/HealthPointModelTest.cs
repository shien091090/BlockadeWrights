using NUnit.Framework;
using Sirenix.Utilities;

namespace GameCore.Tests.HealthPoint
{
    public class HealthPointModelTest
    {
        private HealthPointModel healthPointModel;

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

        private void GivenInitModel(float maxHp)
        {
            healthPointModel = new HealthPointModel(maxHp);
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

        //扣血超出目前HP
        //補血
        //補血超出HP上限
    }
}