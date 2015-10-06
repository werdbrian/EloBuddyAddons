using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;

using Settings = JokerFioraBuddy.Config.Modes.LaneClear;

namespace JokerFioraBuddy.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);

            foreach (var minion in minions)
            {
                if (minion.IsValidTarget(Q.Range) && Settings.UseQ && Q.IsReady() && Player.Instance.ManaPercent > Settings.Mana && minion.Health <= Player.Instance.GetSpellDamage(minion, SpellSlot.Q))
                    Q.Cast(minion);

                if (Settings.UseTiamatHydra)
                {
                    if (PermaActive.Hydra != null && PermaActive.Hydra.IsReady() && minion.IsValidTarget(PermaActive.Hydra.Range - 20) && !minion.IsZombie)
                    {
                        PermaActive.Hydra.Cast();
                        if (minion.IsValidTarget(Player.Instance.GetAutoAttackRange()))
                        {
                            Orbwalker.ResetAutoAttack();
                            Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                        }
                    }
                }

                if (Settings.UseE && E.IsReady() && Player.Instance.ManaPercent > Settings.Mana && minion.Health <= 2 * Player.Instance.GetAutoAttackDamage(Player.Instance) && minion.IsValidTarget(E.Range))
                {
                    E.Cast();
                    if (minion.IsValidTarget(Player.Instance.GetAutoAttackRange()))
                    {
                        Orbwalker.ResetAutoAttack();
                        Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                    }
                }
            }
        }
    }
}
