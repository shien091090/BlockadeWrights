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

            Assert.AreEqual(true, fortressModel.IsInValid);
        }

        [Test]
        //主堡被攻擊, 但尚未被破壞
        public void fortress_be_attacked_but_not_destroyed()
        {
            GivenInitModel(100);

            fortressModel.Damage(10);

            fortressDestroyEvent.DidNotReceive().Invoke();
        }

        private void GivenInitModel(int mapHp)
        {
            fortressModel = new FortressModel(mapHp);
        }
        //主堡被攻擊並且被破壞
    }
}