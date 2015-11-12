using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Events;
using SharpDX;
using System.Drawing;
using JokerFioraBuddy.Modes;
using JokerFioraBuddy.Evade;

using PermaSettings = JokerFioraBuddy.Config.Modes.Perma;
using ComboSettings = JokerFioraBuddy.Config.Modes.Combo;
using ShieldSettings = JokerFioraBuddy.Config.ShieldBlock;

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
            TargetSelector2.Initialize();
            ModeManager.Initialize();
            ItemManager.Initialize();
            SpellManager.Initialize();
            PassiveManager.Initialize();
            SpellBlock.Initialize();

            Text = new Text("", new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold)) { Color = System.Drawing.Color.Red };

            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            Chat.Print("<font color = \"#6B9FE3\">Joker Fiora 1.06</font><font color = \"#E3AF6B\"> by JokerArt</font>. Report any bugs please! Thanks and enjoy.");
        }

        static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var unit = sender as AIHeroClient;

            if (unit == null || !unit.IsValid)
            {
                return;
            }

            if (unit.IsMe && args.Slot.Equals(SpellSlot.E))
            {
                Orbwalker.ResetAutoAttack();
                return;
            }

            if (!unit.IsEnemy || !ShieldSettings.BlockSpells || !SpellManager.W.IsReady())
            {
                return;
            }

            // spell handled by evade
            if (SpellDatabase.GetByName(args.SData.Name) != null)
                return;

            if (!SpellBlock.Contains(unit, args))
                return;

            var castUnit = unit;
            var type = args.SData.TargettingType;

            if (!unit.IsValidTarget())
            {
                var target = TargetSelector2.GetTarget(SpellManager.W.Range - 25, DamageType.Magical);
                if (target == null || !target.IsValidTarget(SpellManager.W.Range))
                {
                    target = TargetSelector.SelectedTarget;
                }

                if (target != null && target.IsValidTarget(SpellManager.W.Range))
                {
                    castUnit = target;
                }
            }

            if (args.End.Distance(Player.Instance) < 60)
            {
                if (unit.ChampionName.Equals("Bard") && args.End.Distance(Player.Instance) < 300)
                {
                    Core.DelayAction(() => CastW(castUnit), (int)(unit.Distance(Player.Instance) / 7f)+ 400 );
                }
                else if (args.End.Distance(Player.Instance) < 60)
                {
                    CastW(castUnit);
                }
            }
            if (args.Target != null)
            {
                if (!args.Target.IsMe ||
                    (args.Target.Name.Equals("Barrel") && args.Target.Distance(Player.Instance) > 200 &&
                     args.Target.Distance(Player.Instance) < 400))
                {
                    return;
                }

                if (unit.ChampionName.Equals("Nautilus") ||
                    (unit.ChampionName.Equals("Caitlyn") && args.Slot.Equals(SpellSlot.R)))
                {
                    var d = unit.Distance(Player.Instance);
                    var travelTime = d / args.SData.MissileSpeed;
                    var delay = travelTime * 1000 - SpellManager.W.CastDelay + 150;
                    Console.WriteLine("TT: " + travelTime + " " + delay);
                    Core.DelayAction(() => CastW(castUnit), (int)(delay));
                    return;
                }

                CastW(castUnit);
            }
            else if (type.Equals(SpellDataTargetType.LocationAoe) && args.End.Distance(Player.Instance) < args.SData.CastRadius)
            {
                // annie moving tibbers
                if (unit.ChampionName.Equals("Annie") && args.Slot.Equals(SpellSlot.R))
                {
                    return;
                }
                CastW(castUnit);

            }
            else if (type.Equals(SpellDataTargetType.Cone) && args.End.Distance(Player.Instance) < args.SData.CastRadius)
            {
                CastW(castUnit);
            }
            else if (type.Equals(SpellDataTargetType.SelfAoe))
            {
                var d = args.End.Distance(Player.Instance.ServerPosition);
                var p = args.SData.CastRadius;
                Console.WriteLine(d + " " + " " + p);
                if (d < p)
                    CastW(castUnit);
            }
        }

        public static bool CastW(Obj_AI_Base target)
        {
            if (target == null || !target.IsValidTarget(SpellManager.W.Range))
                return SpellManager.W.Cast(Game.CursorPos);

            var cast = SpellManager.W.GetPrediction(target);
            var castPos = SpellManager.W.IsInRange(cast.CastPosition) ? cast.CastPosition : target.ServerPosition;

            return SpellManager.W.Cast(castPos);
        }
    }
}
