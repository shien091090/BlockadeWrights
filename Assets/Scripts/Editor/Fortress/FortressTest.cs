using NUnit.Framework;

namespace GameCore.Tests.Fortress
{
    public class FortressTest
    {
        [Test]
        //主堡未設置血量
        public void fortress_no_hp()
        {
            FortressModel fortressModel = new FortressModel(0);

            Assert.AreEqual(true, fortressModel.IsInValid);
        }
        
        //主堡被攻擊, 但尚未被破壞
        //主堡被攻擊並且被破壞
    }
}