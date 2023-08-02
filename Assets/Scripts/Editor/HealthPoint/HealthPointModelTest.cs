using NUnit.Framework;

namespace GameCore.Tests.HealthPoint
{
    public class HealthPointModelTest
    {
        [Test]
        //HP上限為0
        public void max_hp_is_0()
        {
            HealthPointModel healthPointModel = new HealthPointModel(0);
            Assert.IsFalse(healthPointModel.IsValid);
        }

        [Test]
        [TestCase(10, 5, 5)]
        [TestCase(90.5f, 0.5f, 90f)]
        [TestCase(80, 0, 80)]
        //扣血
        public void decrease_hp(float maxHp, float damageValue, float expectedRemainHp)
        {
            HealthPointModel healthPointModel = new HealthPointModel(maxHp);
            healthPointModel.Damage(damageValue);
            Assert.AreEqual(expectedRemainHp, healthPointModel.CurrentHp);
        }

        //扣血至HP歸0
        //扣血超出目前HP
        //補血
        //補血超出HP上限
    }
}