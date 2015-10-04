using EloBuddy;
using EloBuddy.SDK;
using System;
using Settings = JokerFioraBuddy.Config.Modes.Combo;

namespace JokerFioraBuddy.Modes
{
    public sealed class Combo : ModeBase
    {
        public static Spell.Active Tiamat { get; private set; }
        public static Spell.Active Hydra { get; private set; }
        public static Spell.Targeted BOTRK { get; private set; }
        public static Spell.Targeted Cutlass { get; private set; }

        public static ItemId TiamatID { get; private set; }
        public static ItemId HydraID { get; private set; }
        public static ItemId BOTRKID { get; private set; }
        public static ItemId CutlassID { get; private set; }

        public override bool ShouldBeExecuted()
        {
            // Only execute this mode when the orbwalker is on combo mode
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
                foreach (InventorySlot item in ObjectManager.Player.InventoryItems)
                {
                    if (item.DisplayName.Contains("Ruined King"))
                    {
                        BOTRK = new Spell.Targeted(item.SpellSlot, 550);
                        BOTRKID = item.Id;
                        Cutlass = null;
                    }

                    else if (item.DisplayName.Contains("Bilgewater Cutlass"))
                    {
                        Cutlass = new Spell.Targeted(item.SpellSlot, 550);
                        CutlassID = item.Id;
                    }

                    if (item.DisplayName.Contains("Hydra"))
                    {
                        Hydra = new Spell.Active(item.SpellSlot, 400);
                        HydraID = item.Id;
                        Tiamat = null;
                    }

                    else if (item.DisplayName.Contains("Tiamat"))
                    {
                        Tiamat = new Spell.Active(item.SpellSlot, 385);
                        TiamatID = item.Id;
                    }
                    
                }

            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

            if (target != null && target.IsValidTarget(R.Range))
            {
                if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie)
                    Q.Cast(target);

                if (Settings.UseCutlassBOTRK)
                {
                    if (BOTRK != null && BOTRK.IsReady() && target.IsValidTarget(BOTRK.Range) && !target.IsZombie)
                        BOTRK.Cast(target);
                    else if (Cutlass != null && Cutlass.IsReady() && target.IsValidTarget(Cutlass.Range) && !target.IsZombie)
                        Cutlass.Cast(target);
                }
                    
                if (Settings.UseE && E.IsReady() && target.IsValidTarget(E.Range) && !target.IsZombie)
                    E.Cast();

                if (Settings.UseW && W.IsReady() && target.IsValidTarget(W.Range) && !target.IsZombie)
                    W.Cast(target);

                if (Settings.UseTiamatHydra)
                {
                    if (Hydra != null && Hydra.IsReady() && target.IsValidTarget(Hydra.Range) && !target.IsZombie)
                        Hydra.Cast();
                    else if (Tiamat != null && Tiamat.IsReady() && target.IsValidTarget(Tiamat.Range) && !target.IsZombie)
                        Tiamat.Cast();
                }

                if (Settings.UseR && R.IsReady() && target.IsValidTarget(R.Range) && !target.IsZombie)
                    R.Cast(target);
            }
        }
    }
}