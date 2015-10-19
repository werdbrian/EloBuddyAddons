using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;

using Settings = JokerQuinnBuddy.Config.Modes.LaneClear;

namespace JokerQuinnBuddy.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead && m.IsValid && m.IsVisible);

            var qCastPos = EloBuddy.SDK.Prediction.Position.PredictCircularMissileAoe(minions.ToArray(), SpellManager.Q.Range, SpellManager.Q.Radius, SpellManager.Q.CastDelay, SpellManager.Q.Speed);

            if(qCastPos.Count() > 0 && Settings.UseQ && Player.Instance.ManaPercent > Settings.Mana)
                SpellManager.Q.Cast(qCastPos.FirstOrDefault().CastPosition);

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
