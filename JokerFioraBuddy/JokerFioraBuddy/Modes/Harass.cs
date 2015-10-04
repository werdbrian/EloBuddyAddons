using EloBuddy;
using EloBuddy.SDK;
using Settings = JokerFioraBuddy.Config.Modes.Harass;

namespace JokerFioraBuddy.Modes
{
    public sealed class Harass : ModeBase
    {
        public static Spell.Active Tiamat { get; private set; }
        public static Spell.Active Hydra { get; private set; }

        public static ItemId TiamatID { get; private set; }
        public static ItemId HydraID { get; private set; }

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {

            foreach (InventorySlot item in ObjectManager.Player.InventoryItems)
            {
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

            if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie && Player.Instance.ManaPercent > Settings.Mana)
                Q.Cast(target);

            if (Settings.UseTiamatHydra)
            {
                if (Hydra != null && Hydra.IsReady() && target.IsValidTarget(Hydra.Range) && !target.IsZombie)
                    Hydra.Cast();
                else if (Tiamat != null && Tiamat.IsReady() && target.IsValidTarget(Tiamat.Range) && !target.IsZombie)
                    Tiamat.Cast();
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
