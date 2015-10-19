using System;
using EloBuddy;
using EloBuddy.SDK;

using Settings = JokerQuinnBuddy.Config.Modes.Combo;

namespace JokerQuinnBuddy.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector2.GetTarget(Quinn.qRange, DamageType.Physical);

            if (target == null || !target.IsValidTarget())
                return;

            if (target.IsInAutoAttackRange(Player.Instance))
            {
                Orbwalker.ForcedTarget = target;
                Player.IssueOrder(GameObjectOrder.AutoAttack, target);
            }

            if (SpellManager.R.IsReady() && target.IsValidTarget(1200) && Quinn.CalcDamage(target) > target.Health - 20 * Player.Instance.Level ||
                SpellManager.R.IsReady() && target.IsValidTarget(1200) && Quinn.DiveDmgCalc(target) > target.Health && Settings.UseR)
                Quinn.rLogic();

            if (Player.HasBuff("quinnrtimeout") || Player.HasBuff("QuinnRForm"))
                Quinn.ASMode();

            if (Player.HasBuff("quinnrtimeout") || Player.HasBuff("QuinnRForm"))
                return;

            if (SpellManager.Q.IsReady() && target.IsValidTarget(Quinn.qRange))
                SpellManager.Q.Cast(target);
        }
    }
}
