using System.Linq;
using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;

using Settings = JokerFioraBuddy.Config.Modes.Perma;
using ComboSettings = JokerFioraBuddy.Config.Modes.Combo;
using SharpDX;


namespace JokerFioraBuddy.Modes
{
    public sealed class PermaActive : ModeBase
    {

        public override bool ShouldBeExecuted()
        {
            return true;
        }
        
        public override void Execute()
        {
            DamageIndicator.DamageToUnit = GetComboDamage;

            if (ObjectManager.Player.IsDead || !IG.IsReady() || !Settings.UseIgnite) return;
            if (ObjectManager.Get<AIHeroClient>().Where(
                        h =>
                            h.IsValidTarget(IG.Range) &&
                            h.Health <
                            ObjectManager.Player.GetSummonerSpellDamage(h, DamageLibrary.SummonerSpells.Ignite)).Count() <= 0) return;

            var target = ObjectManager.Get<AIHeroClient>()
                .Where(
                        h =>
                            h.IsValidTarget(IG.Range) &&
                            h.Health <
                            ObjectManager.Player.GetSummonerSpellDamage(h, DamageLibrary.SummonerSpells.Ignite));

            IG.Cast(target.First());
                
        }

        private static float GetComboDamage(AIHeroClient unit)
        {
            return GetComboDamage(unit, 0);
        }

        private static float GetComboDamage(AIHeroClient unit, int maxStacks)
        {
            var d = 2 * Player.Instance.GetAutoAttackDamage(unit);

            if (Combo.BOTRK != null && Combo.BOTRK.IsReady())
                d += Player.Instance.GetItemDamage(unit, Combo.BOTRKID);

            if (Combo.Cutlass != null && Combo.Cutlass.IsReady())
                d += Player.Instance.GetItemDamage(unit, Combo.CutlassID);

            if (Combo.Hydra != null && Combo.Hydra.IsReady())
                d += Player.Instance.GetItemDamage(unit, Combo.HydraID);

            if (Combo.Tiamat != null && Combo.Tiamat.IsReady())
                d += Player.Instance.GetItemDamage(unit, Combo.TiamatID);

            if (Settings.UseIgnite && SpellManager.IG.IsReady())
                d += Player.Instance.GetSummonerSpellDamage(unit, DamageLibrary.SummonerSpells.Ignite);

            if (ComboSettings.UseQ && SpellManager.Q.IsReady())
                d += Player.Instance.GetSpellDamage(unit, SpellSlot.Q);

            if (ComboSettings.UseW && SpellManager.W.IsReady())
                d += Player.Instance.GetSpellDamage(unit, SpellSlot.W);

            if (ComboSettings.UseE && SpellManager.E.IsReady())
                d += 2 * Player.Instance.GetAutoAttackDamage(unit);

            if (maxStacks == 0)
            {
                if (SpellManager.R.IsReady())
                    d += (float)unit.GetPassiveDamage(4);
                else
                    d += (float)unit.GetPassiveDamage();
            }
            else
                d += (float)unit.GetPassiveDamage(maxStacks);
            if (SpellManager.R.IsReady())
                d += Player.Instance.GetSpellDamage(unit, SpellSlot.R);

            return (float)d;

        }
    }
}
