using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.AutoAttack
{
    public class AutoAttackModelTest
    {
        private AutoAttackModel autoAttackModel;

        [SetUp]
        public void Setup()
        {
            autoAttackModel = new AutoAttackModel(10, 1);
        }

        [Test]
        //未進入攻擊範圍, 不攻擊
        public void not_in_attack_range()
        {
            AttackTargetCountShouldBe(0);
        }

        [Test]
        //進入攻擊範圍, 但未設定攻擊頻率, 不攻擊
        public void in_attack_range_but_not_set_attack_frequency()
        {
            autoAttackModel = new AutoAttackModel(5, 0);

            IAttackTarget attackTarget = Substitute.For<IAttackTarget>();
            autoAttackModel.AddAttackTarget(attackTarget);

            autoAttackModel.UpdateAttackTimer(1);

            ShouldDamageTarget(attackTarget, 0);
        }

        private void AttackTargetCountShouldBe(int expectedCount)
        {
            Assert.AreEqual(expectedCount, autoAttackModel.AttackTargets.Count);
        }

        private void ShouldDamageTarget(IAttackTarget attackTarget, int triggerTimes)
        {
            if (triggerTimes == 0)
                attackTarget.DidNotReceive().Damage(Arg.Any<float>());
            else
                attackTarget.Received(triggerTimes).Damage(Arg.Any<float>());
        }

        //進入攻擊範圍, 攻擊, CD時間結束後對象離開攻擊範圍, 不攻擊
        //進入攻擊範圍, 攻擊, CD時間結束後對象尚未離開攻擊範圍, 再攻擊一次
        //進入攻擊範圍, 攻擊, CD時間結束前對象離開後又進入攻擊範圍, 再攻擊一次
        //攻擊後, 對象死亡, 不再攻擊
        //攻擊範圍中有多個目標, 攻擊距離最近的對象
        //攻擊範圍中最近目標, CD時間結束後有其他更近的目標, 攻擊距離最近的對象
        //停止自動攻擊
    }
}