using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Settings = JokerFioraBuddy.Config.Modes.Harass;

namespace JokerFioraBuddy.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (target != null && target.IsValidTarget(Q.Range))
            {

                if (PassiveManager.GetPassivePosition(target) != Vector3.Zero)
                {
                    if (target.IsValidTarget(Player.Instance.GetAutoAttackRange()))
                    {
                        Orbwalker.OrbwalkTo(PassiveManager.GetPassivePosition(target));
                        Player.IssueOrder(GameObjectOrder.AttackUnit, PassiveManager.GetPassivePosition(target));
                    }
                }

                if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie && Player.Instance.ManaPercent > Settings.Mana)
                {
                    if (PassiveManager.GetPassivePosition(target) != Vector3.Zero)
                        Q.Cast(PassiveManager.GetPassivePosition(target));
                    else
                        Q.Cast(target);
                }

                if (Settings.UseTiamatHydra)
                {
                    if (PermaActive.Hydra != null && PermaActive.Hydra.IsReady() && target.IsValidTarget(PermaActive.Hydra.Range) && !target.IsZombie)
                        PermaActive.Hydra.Cast();
                }


                if (Settings.UseE && E.IsReady() && target.IsValidTarget(E.Range) && !target.IsZombie && Player.Instance.ManaPercent > Settings.Mana)
                    E.Cast();

                if (Settings.UseW && W.IsReady() && target.IsValidTarget(W.Range) && !target.IsZombie && Player.Instance.ManaPercent > Settings.Mana)
                    W.Cast(target);

                if (Settings.UseR && R.IsReady() && target.IsValidTarget(R.Range) && !target.IsZombie && Player.Instance.ManaPercent > Settings.Mana)
                    R.Cast(target);
            }
        }
    }
}
