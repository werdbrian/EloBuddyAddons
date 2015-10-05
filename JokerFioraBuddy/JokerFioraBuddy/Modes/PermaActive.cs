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

        public static Spell.Active Hydra { get; private set; }
        public static Spell.Targeted BOTRK { get; private set; }
        public static Spell.Targeted Cutlass { get; private set; }
        public static Spell.Active Youmuus { get; private set; }
        public static Spell.Active Sheen { get; private set; }
        public static Spell.Active TriForce { get; private set; }

        public static ItemId HydraID { get; private set; }
        public static ItemId BOTRKID { get; private set; }
        public static ItemId CutlassID { get; private set; }
        public static ItemId YomuusID { get; private set; }
        public static ItemId SheenID { get; private set; }
        public static ItemId TriForceID { get; private set; }


        public override bool ShouldBeExecuted()
        {
            return true;
        }
        
        public override void Execute()
        {
            if(Hydra == null || BOTRK == null || Cutlass == null || Youmuus == null || TriForce == null)
            {
                foreach (InventorySlot item in ObjectManager.Player.InventoryItems)
                {
                    if (item.DisplayName.Contains("Trinity Force") && TriForce == null)
                    {
                        TriForce = new Spell.Active(item.SpellSlot);
                        TriForceID = item.Id;
                        continue;
                    }

                    else if (item.DisplayName.Contains("Sheen") && Sheen == null)
                    {
                        Sheen = new Spell.Active(item.SpellSlot);
                        SheenID = item.Id;
                        continue;
                    }

                    if (item.DisplayName.Contains("Youmuu") && Youmuus == null)
                    {
                        Youmuus = new Spell.Active(item.SpellSlot);
                        YomuusID = item.Id;
                        continue;
                    }

                    if (item.DisplayName.Contains("Ruined King") && BOTRK == null)
                    {
                        BOTRK = new Spell.Targeted(item.SpellSlot, 550);
                        BOTRKID = item.Id;
                        Cutlass = null;
                        continue;
                    }

                    else if (item.DisplayName.Contains("Bilgewater Cutlass") && Cutlass == null)
                    {
                        Cutlass = new Spell.Targeted(item.SpellSlot, 550);
                        CutlassID = item.Id;
                        continue;
                    }

                    if (item.DisplayName.Contains("Ravenous Hydra") && Hydra == null)
                    {
                        Hydra = new Spell.Active(item.SpellSlot, 400);
                        HydraID = item.Id;
                        continue;
                    }

                    else if (item.DisplayName.Contains("Tiamat") && Hydra == null)
                    {
                        Hydra = new Spell.Active(item.SpellSlot, 400);
                        HydraID = EloBuddy.ItemId.Ravenous_Hydra_Melee_Only;
                        continue;
                    }
                }
            }

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

            if (BOTRK != null && BOTRK.IsReady())
                d += Player.Instance.GetItemDamage(unit, BOTRKID);

            if (Cutlass != null && Cutlass.IsReady())
                d += Player.Instance.GetItemDamage(unit, CutlassID);

            if (Hydra != null && Hydra.IsReady())
                d += Player.Instance.GetItemDamage(unit, HydraID);

            if (Sheen != null && Sheen.IsReady())
                d += Player.Instance.GetAutoAttackDamage(unit) + Player.Instance.BaseAttackDamage * 2;

            if (TriForce != null && TriForce.IsReady())
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
