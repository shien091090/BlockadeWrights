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
        //扣血
        public void decrease_hp()
        {
            HealthPointModel healthPointModel = new HealthPointModel(10);
            healthPointModel.Damage(5);
            Assert.AreEqual(5, healthPointModel.CurrentHp);
        }
        
        //扣血至HP歸0
        //扣血超出目前HP
        //補血
        //補血超出HP上限
    }
}