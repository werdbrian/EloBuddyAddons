using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace JokerFioraBuddy
{
    public static class Config
    {
        private const string MenuName = "Joker Fiora";

        private static readonly Menu Menu;

        static Config() 
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to Joker Fiora Addon!");
            Menu.AddLabel("Features:");
            Menu.AddLabel("- Epic Combo! 100-0 in 2 seconds.");
            Menu.AddLabel("- Harass Mode with all spells.");
            Menu.AddLabel("- Last Hit Mode with Q.");
            Menu.AddLabel("- Lane Clear Mode with Q/E.");
            Menu.AddLabel("- Auto-Ignite!");
            Menu.AddLabel("- Minion last hit indicator and champion 1 shot combo indicator!");
            Menu.AddLabel("All customizable! Featuring Youmuu's Ghostblade / Ravenous Hydra / Blade of the Ruined King");
            Menu.AddLabel("Credits to: Danny - Main Coder. / Trees - Passive Manager");

            Modes.Initialize();
            Drawings.Initialize();
        }

        public static void Initialize() 
        {
 
        }

        public static class Drawings
        {
            private static readonly Menu Menu;

            static Drawings()
            {
                Menu = Config.Menu.AddSubMenu("Drawings");

            }

            public static void Initialize()
            {

            }

            public static class Damage
            {
                public static bool ShowKillable
                {
                    get { return Menu["damageKillable"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool ShowMinionsLS
                {
                    get { return Menu["damageMinionsLS"].Cast<CheckBox>().CurrentValue; }
                }

                static Damage()
                {
                    Menu.AddGroupLabel("Damage");
                    Menu.Add("damageKillable", new CheckBox("Show text if champion is killable"));
                    Menu.Add("damageMinionsLS", new CheckBox("Show green circle if minion dies to last hit"));
                }

                public static void Initialize()
                {

                }
            }
        }

        public static class Modes
        {
            private static readonly Menu Menu;

            static Modes()
            {
                Menu = Config.Menu.AddSubMenu("Modes");

                Combo.Initialize();
                Menu.AddSeparator();

                Harass.Initialize();
                Menu.AddSeparator();

                LaneClear.Initialize();
                Menu.AddSeparator();

                LastHit.Initialize();
                Menu.AddSeparator();

                Perma.Initialize();
            }

            public static void Initialize()
            {

            }

            public static class Combo
            {
                public static bool UseQ
                {
                    get { return Menu["comboUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseW
                {
                    get { return Menu["comboUseW"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseE
                {
                    get { return Menu["comboUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseR
                {
                    get { return Menu["comboUseR"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseTiamatHydra
                {
                    get { return Menu["comboUseTiamatHydra"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseCutlassBOTRK
                {
                    get { return Menu["comboUseCutlassBOTRK"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseYomuus
                {
                    get { return Menu["comboUseYomuus"].Cast<CheckBox>().CurrentValue; }
                }

                static Combo()
                {
                    Menu.AddGroupLabel("Combo");
                    Menu.Add("comboUseQ", new CheckBox("Use Q"));
                    Menu.Add("comboUseW", new CheckBox("Use W"));
                    Menu.Add("comboUseE", new CheckBox("Use E"));
                    Menu.Add("comboUseR", new CheckBox("Use R"));
                    Menu.Add("comboUseTiamatHydra", new CheckBox("Use Tiamat / Hydra"));
                    Menu.Add("comboUseCutlassBOTRK", new CheckBox("Use Bilgewater Cutlass / Blade of the Ruined King"));
                    Menu.Add("comboUseYomuus", new CheckBox("Use Youmuu's Ghostblade"));
                }

                public static void Initialize()
                {

                }
            }

            public static class Harass
            {
                public static bool UseQ
                {
                    get { return Menu["harassUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseW
                {
                    get { return Menu["harassUseW"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseE
                {
                    get { return Menu["harassUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseR
                {
                    get { return Menu["harassUseR"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseTiamatHydra
                {
                    get { return Menu["harassUseTiamatHydra"].Cast<CheckBox>().CurrentValue; }
                }

                public static int Mana
                {
                    get { return Menu["harassMana"].Cast<Slider>().CurrentValue; }
                }

                static Harass()
                {
                    Menu.AddGroupLabel("Harrass");
                    Menu.Add("harassUseQ", new CheckBox("Use Q"));
                    Menu.Add("harassUseW", new CheckBox("Use W"));
                    Menu.Add("harassUseE", new CheckBox("Use E"));
                    Menu.Add("harassUseR", new CheckBox("Use R", false));
                    Menu.Add("harassUseTiamatHydra", new CheckBox("Use Tiamat / Hydra"));
                    Menu.Add("harassMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
                }

                public static void Initialize()
                {

                }
            }

            public static class LaneClear
            {
                public static bool UseQ
                {
                    get { return Menu["lcUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseE
                {
                    get { return Menu["lcUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static int Mana
                {
                    get { return Menu["lcMana"].Cast<Slider>().CurrentValue; }
                }

                static LaneClear()
                {
                    Menu.AddGroupLabel("Lane Clear");
                    Menu.Add("lcUseQ", new CheckBox("Use Q"));
                    Menu.Add("lcUseE", new CheckBox("Use E"));
                    Menu.Add("lcMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
                }

                public static void Initialize()
                {

                }
            }

            public static class LastHit
            {
                public static bool UseQ
                {
                    get { return Menu["lhUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                public static int Mana
                {
                    get { return Menu["lhMana"].Cast<Slider>().CurrentValue; }
                }

                static LastHit()
                {
                    Menu.AddGroupLabel("Last Hit");
                    Menu.Add("lhUseQ", new CheckBox("Use Q"));
                    Menu.Add("lhMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
                }

                public static void Initialize()
                {

                }
            }

            public static class Flee
            {
                public static bool UseQ
                {
                    get { return Menu["fleeUseQ"].Cast<CheckBox>().CurrentValue; }
                }

                static Flee()
                {
                    Menu.AddGroupLabel("Flee");
                    Menu.Add("fleeUseQ", new CheckBox("Use Q"));
                }

                public static void Initialize()
                {

                }
            }

            public static class Perma
            {
                public static bool UseIgnite
                {
                    get { return Menu["permaUseIG"].Cast<CheckBox>().CurrentValue; }
                }

                static Perma()
                {
                    Menu.AddGroupLabel("Perma Active");
                    Menu.Add("permaUseIG", new CheckBox("Auto-Ignite Champions"));
                }

                public static void Initialize()
                {

                }
            }
        }
    }
}
