using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System.Drawing;
using JokerFioraBuddy.Modes;

using PermaSettings = JokerFioraBuddy.Config.Modes.Perma;
using ComboSettings = JokerFioraBuddy.Config.Modes.Combo;
using EloBuddy.SDK;

namespace JokerFioraBuddy
{
    public static class Program
    {
        public const string ChampName = "Fiora";
        private static Text Text { get; set; }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName)
                return;

            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();

            Text = new Text("", new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold)) { Color = System.Drawing.Color.Red };
            
            Drawing.OnDraw += OnDraw;
        }

        static void OnDraw(EventArgs args)
        {

        }
    }
}
