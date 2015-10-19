using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace JokerQuinnBuddy
{
    public static class ItemManager
    {
        public static Item BOTRK { get; private set; }
        public static Item Cutl { get; private set; }
        public static Item Yomu { get; private set; }

        static ItemManager()
        {
            BOTRK = new Item((int)ItemId.Blade_of_the_Ruined_King, 450);
            Cutl = new Item((int)ItemId.Bilgewater_Cutlass, 450);
            Yomu = new Item((int)ItemId.Youmuus_Ghostblade);
        }

        public static void UseYomu()
        {
            if (Yomu.IsOwned() && Yomu.IsReady())
                Yomu.Cast();
        }

        public static void UseCastables()
        {
            if (BOTRK.IsOwned() || Cutl.IsOwned())
            {
                var t = TargetSelector2.GetTarget(BOTRK.Range, DamageType.Physical);
                if (t == null || !t.IsValidTarget()) return;

                if (BOTRK.IsReady() || Cutl.IsReady())
                {
                    BOTRK.Cast(t);
                    Cutl.Cast(t);
                }
            }
        }

        public static void Initialize()
        {

        }
    }
}
