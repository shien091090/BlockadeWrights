using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.AutoAttack
{
    public class AutoAttackModelTest
    {
        private const int DEFAULT_ATTACK_RANGE = 10;
        private const int DEFAULT_ATTACK_FREQUENCY = 1;
        private const int DEFAULT_ATTACK_POWER = 1;

        private AutoAttackModel autoAttackModel;

        [Test]
        //未進入攻擊範圍, 不攻擊
        public void not_in_attack_range()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER);

            AttackTargetCountShouldBe(0);
        }

        [Test]
        //進入攻擊範圍, 但未設定攻擊頻率, 不攻擊
        public void in_attack_range_but_not_set_attack_frequency()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, 0, Vector2.zero, DEFAULT_ATTACK_POWER);

            IAttackTarget attackTarget = Substitute.For<IAttackTarget>();
            autoAttackModel.AddAttackTarget(attackTarget);

            autoAttackModel.UpdateAttackTimer(1);

            ShouldDamageTarget(attackTarget, 0);
        }

        [Test]
        //進入攻擊範圍, 攻擊
        public void in_attack_range_then_attack()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER);

            IAttackTarget attackTarget = Substitute.For<IAttackTarget>();
            autoAttackModel.AddAttackTarget(attackTarget);

            autoAttackModel.UpdateAttackTimer(0.5f);

            ShouldDamageTarget(attackTarget, 0);

            autoAttackModel.UpdateAttackTimer(0.5f);

            ShouldDamageTarget(attackTarget, 1);
        }

        [Test]
        //進入攻擊範圍, 攻擊, CD時間結束後對象離開攻擊範圍, 不攻擊
        public void in_attack_range_then_attack_then_target_leave_attack_range()
        {
            autoAttackModel = new AutoAttackModel(4, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER);

            IAttackTarget attackTarget = Substitute.For<IAttackTarget>();
            GivenAttackTargetPos(attackTarget, new Vector2(2, 3));

            autoAttackModel.AddAttackTarget(attackTarget);
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
            ShouldDamageTarget(attackTarget, 1);

            GivenAttackTargetPos(attackTarget, new Vector2(3, 4));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(0);
            ShouldDamageTarget(attackTarget, 1);
        }

        [Test]
        //進入攻擊範圍, 攻擊, CD時間結束後對象尚未離開攻擊範圍, 再攻擊一次
        public void in_attack_range_then_attack_then_target_still_in_attack_range()
        {
            autoAttackModel = new AutoAttackModel(5, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER);

            IAttackTarget attackTarget = Substitute.For<IAttackTarget>();
            GivenAttackTargetPos(attackTarget, new Vector2(2, 3));

            autoAttackModel.AddAttackTarget(attackTarget);
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
            ShouldDamageTarget(attackTarget, 1);

            GivenAttackTargetPos(attackTarget, new Vector2(3, 4));
            autoAttackModel.UpdateAttackTimer(DEFAULT_ATTACK_FREQUENCY);

            AttackTargetCountShouldBe(1);
            ShouldDamageTarget(attackTarget, 2);
        }

        [Test]
        //進入攻擊範圍, 攻擊, CD時間結束前對象離開後又進入攻擊範圍, 再攻擊一次
        public void in_attack_range_then_attack_then_target_leave_attack_range_then_enter_attack_range()
        {
            autoAttackModel = new AutoAttackModel(4, 1, Vector2.zero, DEFAULT_ATTACK_POWER);

            IAttackTarget attackTarget = Substitute.For<IAttackTarget>();
            GivenAttackTargetPos(attackTarget, new Vector2(2, 3));

            autoAttackModel.AddAttackTarget(attackTarget);
            autoAttackModel.UpdateAttackTimer(1);

            AttackTargetCountShouldBe(1);
            ShouldDamageTarget(attackTarget, 1);

            GivenAttackTargetPos(attackTarget, new Vector2(3, 4));
            autoAttackModel.UpdateAttackTimer(0.5f);

            AttackTargetCountShouldBe(1);

            GivenAttackTargetPos(attackTarget, new Vector2(2, 3));
            autoAttackModel.UpdateAttackTimer(0.5f);

            AttackTargetCountShouldBe(1);
            ShouldDamageTarget(attackTarget, 2);
        }

        [Test]
        //同一個目標進出攻擊範圍多次, 驗證攻擊目標列表中是否只有一個目標
        public void same_target_enter_and_leave_attack_range_multiple_times()
        {
            autoAttackModel = new AutoAttackModel(DEFAULT_ATTACK_RANGE, DEFAULT_ATTACK_FREQUENCY, Vector2.zero, DEFAULT_ATTACK_POWER);

            IAttackTarget attackTarget = Substitute.For<IAttackTarget>();

            autoAttackModel.AddAttackTarget(attackTarget);
            autoAttackModel.RemoveAttackTarget(attackTarget);
            autoAttackModel.AddAttackTarget(attackTarget);
            autoAttackModel.RemoveAttackTarget(attackTarget);
            autoAttackModel.AddAttackTarget(attackTarget);
            autoAttackModel.AddAttackTarget(attackTarget);

            AttackTargetCountShouldBe(1);
        }

        private void GivenAttackTargetPos(IAttackTarget attackTarget, Vector2 targetPos)
        {
            attackTarget.GetPos.Returns(targetPos);
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

        //多個目標進入攻擊範圍後離開, 只剩一個攻擊目標
        //攻擊後, 對象死亡, 不再攻擊
        //攻擊範圍中有多個目標, 攻擊距離最近的對象
        //攻擊範圍中最近目標, CD時間結束後有其他更近的目標, 攻擊距離最近的對象
        //停止自動攻擊
    }
}