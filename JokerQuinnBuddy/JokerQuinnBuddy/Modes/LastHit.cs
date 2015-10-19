using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;

namespace JokerQuinnBuddy.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead && m.IsValid && m.IsVisible);
            foreach (var minion in minions)
            {
                if (minion.HasBuff("QuinnW") && minion.Health < Player.Instance.CalculateDamageOnUnit(minion, DamageType.Physical,
                    (float)(15 + (Player.Instance.Level * 10) + (Player.Instance.FlatPhysicalDamageMod * 0.5) + Player.Instance.GetAutoAttackDamage(minion))) && minion.IsValidTarget())
                {
                    Orbwalker.ForcedTarget = minion;
                    Player.IssueOrder(GameObjectOrder.AutoAttack, minion);
                }
            }
        }
    }
}
