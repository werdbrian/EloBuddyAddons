using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using System;
using Settings = JokerFioraBuddy.Config.Modes.Combo;

namespace JokerFioraBuddy.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {

            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (target != null && target.IsValidTarget(R.Range))
            {

                if (PassiveManager.GetPassivePosition(target) != Vector3.Zero)
                {
                    if (target.IsValidTarget(Player.Instance.GetAutoAttackRange()))
                    {
                        Orbwalker.OrbwalkTo(PassiveManager.GetPassivePosition(target));
                        Player.IssueOrder(GameObjectOrder.AttackUnit, PassiveManager.GetPassivePosition(target));
                    }
                }

                if (Settings.UseYomuus)
                {
                    if (PermaActive.Youmuus != null && PermaActive.Youmuus.IsReady())
                    {
                        PermaActive.Youmuus.Cast();
                    }
                }

                if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie)
                {
                    if (PassiveManager.GetPassivePosition(target) != Vector3.Zero)
                        Q.Cast(PassiveManager.GetPassivePosition(target));
                    else
                        Q.Cast(target);
                }

                if (Settings.UseCutlassBOTRK)
                {
                    if (PermaActive.BOTRK != null && PermaActive.BOTRK.IsReady() && target.IsValidTarget(PermaActive.BOTRK.Range) && !target.IsZombie)
                        PermaActive.BOTRK.Cast(target);
                    else if (PermaActive.Cutlass != null && PermaActive.Cutlass.IsReady() && target.IsValidTarget(PermaActive.Cutlass.Range) && !target.IsZombie)
                        PermaActive.Cutlass.Cast(target);
                }
                
                if (Settings.UseTiamatHydra)
                {
                    if (PermaActive.Hydra != null && PermaActive.Hydra.IsReady() && target.IsValidTarget(PermaActive.Hydra.Range) && !target.IsZombie)
                        PermaActive.Hydra.Cast();
                }

                if (Settings.UseE && E.IsReady() && target.IsValidTarget(E.Range) && !target.IsZombie)
                    E.Cast();

                if (Settings.UseW && W.IsReady() && target.IsValidTarget(W.Range) && !target.IsZombie)
                    W.Cast(target);


                if (Settings.UseR && R.IsReady() && target.IsValidTarget(R.Range) && !target.IsZombie)
                    R.Cast(target);
            }
        }
    }
}