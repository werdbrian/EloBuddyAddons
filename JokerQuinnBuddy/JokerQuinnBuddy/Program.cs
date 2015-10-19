using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Events;
using SharpDX;
using System.Drawing;
using JokerQuinnBuddy.Modes;

namespace JokerQuinnBuddy
{
    class Program
    {
        public const string ChampName = "Quinn";

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName)
                return;

            Config.Initialize();
            Quinn.Initialize();
            TargetSelector2.Initialize();
            ModeManager.Initialize();
            ItemManager.Initialize();
            SpellManager.Initialize();

            Chat.Print("<font color = \"#6B9FE3\">Joker Quinn 1.00</font><font color = \"#E3AF6B\"> by JokerArt</font>. Report any bugs please! Thanks and enjoy.");
        }
    }
}
