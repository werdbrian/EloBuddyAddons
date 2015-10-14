using System.Linq;
using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using SharpDX;

using Settings = JokerFioraBuddy.Config.Modes.Perma;
using ComboSettings = JokerFioraBuddy.Config.Modes.Combo;



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

            if (ItemManager.BOTRK.IsReady() && ItemManager.BOTRK.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Blade_of_the_Ruined_King);

            if (ItemManager.Cutl.IsReady() && ItemManager.Cutl.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Bilgewater_Cutlass);

            if (ItemManager.Hydra.IsReady() && ItemManager.Hydra.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Ravenous_Hydra_Melee_Only);

            if (ItemManager.Tiamat.IsReady() && ItemManager.Tiamat.IsOwned())
                d += Player.Instance.GetItemDamage(unit, ItemId.Ravenous_Hydra_Melee_Only);

            if (ItemManager.Sheen.IsReady() && ItemManager.Sheen.IsOwned())
                d += Player.Instance.GetAutoAttackDamage(unit) + Player.Instance.BaseAttackDamage * 2;

            if (ItemManager.TriForce.IsReady() && ItemManager.TriForce.IsOwned())
                d += Player.Instance.GetAutoAttackDamage(unit) + Player.Instance.BaseAttackDamage * 3;

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
                    d += (float)PassiveManager.GetPassiveDamage(unit, 4);
                else
                    d += (float)PassiveManager.GetPassiveDamage(unit, PassiveManager.GetPassiveCount(unit));
            }
            else
                d += (float)PassiveManager.GetPassiveDamage(unit, maxStacks);
            if (SpellManager.R.IsReady())
                d += Player.Instance.GetSpellDamage(unit, SpellSlot.R);

            return (float)d;

        }
    }
}
