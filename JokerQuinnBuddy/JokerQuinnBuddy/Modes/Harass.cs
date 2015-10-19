using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

using Settings = JokerQuinnBuddy.Config.Modes.Harass;

namespace JokerQuinnBuddy.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector2.GetTarget(Quinn.qRange, DamageType.Physical);

            if (target == null || !target.IsValidTarget())
                return;

            if (SpellManager.Q.IsReady() && target.IsValidTarget(Quinn.qRange) && Settings.UseQ && Player.Instance.ManaPercent > Settings.Mana)
                SpellManager.Q.Cast(target);

            if(SpellManager.E.IsReady() && target.HasBuff("QuinnW"))
                Player.IssueOrder(GameObjectOrder.AutoAttack, target);

            if (SpellManager.E.IsReady() && target.HasBuff("QuinnW"))
                return;

            if (SpellManager.E.IsReady() && target.IsValidTarget(Quinn.eRange) && Settings.UseE && Player.Instance.ManaPercent > Settings.Mana)
            {
                SpellManager.E.Cast(target);
                Player.IssueOrder(GameObjectOrder.AutoAttack, target);
            }
        }
    }
}
