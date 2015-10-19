using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;


namespace JokerQuinnBuddy
{
    public static class Config
    {
        private const string MenuName = "Joker Quinn";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to Joker Quinn 1.00 Addon!");
            Menu.AddLabel("Features:");
            Menu.AddLabel("Combo Mode:");
            Menu.AddLabel("- Only uses ult when enemy is killable with combo;");
            Menu.AddLabel("- Uses E and Q;");
            Menu.AddLabel("- Uses BOTRK and Yomuus;");
            Menu.AddLabel("Harass Mode:");
            Menu.AddLabel("- Uses Q and E depending on the mana you choose from the scale;");
            Menu.AddLabel("Lane Clear Mode:");
            Menu.AddLabel("- Uses Q on minions depending on the mana you choose from the scale;");
            Menu.AddLabel("- Searches for minions with passive and if they're killable with an auto-attack forces that target;");
            Menu.AddLabel("Last Hit Mode:");
            Menu.AddLabel("- Searches for minions with passive and if they're killable with an auto-attack forces that target;");
            Menu.AddLabel("Auto-Ignites Champions when they die from it;");
            Menu.AddLabel("Automaticly E's champions after auto attacks, good for harrasing;");
            Menu.AddLabel("Credits to: Danny - Main Coder");

            Modes.Initialize();
            Drawings.Initialize();

        }

        public static void Initialize()
        {

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

                public static bool UseE
                {
                    get { return Menu["comboUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static bool UseR
                {
                    get { return Menu["comboUseR"].Cast<CheckBox>().CurrentValue; }
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
                    Menu.Add("comboUseE", new CheckBox("Use E"));
                    Menu.Add("comboUseR", new CheckBox("Use R"));
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

                public static bool UseE
                {
                    get { return Menu["harassUseE"].Cast<CheckBox>().CurrentValue; }
                }

                public static int Mana
                {
                    get { return Menu["harassMana"].Cast<Slider>().CurrentValue; }
                }

                static Harass()
                {
                    Menu.AddGroupLabel("Harrass");
                    Menu.Add("harassUseQ", new CheckBox("Use Q"));
                    Menu.Add("harassUseE", new CheckBox("Use E"));
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
                public static int Mana
                {
                    get { return Menu["lcMana"].Cast<Slider>().CurrentValue; }
                }

                static LaneClear()
                {
                    Menu.AddGroupLabel("Lane Clear");
                    Menu.Add("lcUseQ", new CheckBox("Use Q"));
                    Menu.Add("lcMana", new Slider("Maximum mana usage in percent ({0}%)", 40));
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

                public static bool UseEAfterAA
                {
                    get { return Menu["permaUseE"].Cast<CheckBox>().CurrentValue; }
                }

                static Perma()
                {
                    Menu.AddGroupLabel("Perma Active");
                    Menu.Add("permaUseIG", new CheckBox("Auto-Ignite Champions"));
                    Menu.Add("permaUseE", new CheckBox("Auto E after Auto-Attack"));
                }

                public static void Initialize()
                {

                }
            }
        }

        public static class Drawings
        {
            private static readonly Menu Menu;

            public static bool ShowChampionTarget
            {
                get { return Menu["damageChampionTarget"].Cast<CheckBox>().CurrentValue; }
            }

            static Drawings()
            {
                Menu = Config.Menu.AddSubMenu("Drawings");
                Menu.AddGroupLabel("Drawings");
                Menu.Add("damageChampionTarget", new CheckBox("Show circle below targeted champion"));

            }

            public static void Initialize()
            {

            }
        }

    }
}
