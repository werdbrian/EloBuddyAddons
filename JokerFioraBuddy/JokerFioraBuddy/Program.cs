using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Events;
using SharpDX;
using System.Drawing;
using JokerFioraBuddy.Modes;

using PermaSettings = JokerFioraBuddy.Config.Modes.Perma;
using ComboSettings = JokerFioraBuddy.Config.Modes.Combo;
using ShieldSettings = JokerFioraBuddy.Config;

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
            PassiveManager.Initialize();
            TargetSelector2.Initialize();

            Text = new Text("", new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold)) { Color = System.Drawing.Color.Red };

            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Drawing.OnDraw += OnDraw;
        }

        static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender == null || !sender.IsValid || sender.IsMe)
                return;

            var autoW = SpellManager.W.IsReady();

            if (!autoW)
                return;

            var unit = sender as AIHeroClient;
            var castUnit = unit;
            var type = args.SData.TargettingType;

            var blockableSpell = unit != null && unit.IsEnemy && SpellBlock.Contains(unit, args) && ShieldSettings.UseShieldBlock;
            
            if (!blockableSpell)
                return;

            if (!unit.IsValidTarget())
            {
                var target = TargetSelector2.GetTarget(SpellManager.W.Range, DamageType.Mixed);

                if (target == null || !target.IsValidTarget(SpellManager.W.Range))
                    target = TargetSelector2.GetTarget(SpellManager.W.Range, DamageType.Mixed);

                if (target != null && target.IsValidTarget(SpellManager.W.Range))
                    castUnit = target;
            }

            if (type.IsTargeted() && args.Target != null && args.Target.IsMe)
                CastW(castUnit);

            else if (unit.ChampionName.Equals("Riven") && unit.Distance(Player.Instance) < 400)
                CastW(castUnit);

            else if (unit.ChampionName.Equals("Bard") && type.Equals(SpellDataTargetType.Location) && args.End.Distance(Player.Instance.ServerPosition) < 300)
                CastW(castUnit);

            else if (args.SData.ConsideredAsAutoAttack && args.Target != null && args.Target.IsMe)
                CastW(castUnit);

            else if (type.Equals(SpellDataTargetType.SelfAoe) && unit.Distance(Player.Instance.ServerPosition) < args.SData.CastRange + args.SData.CastRadius / 2)
                CastW(castUnit);

            else if (type.Equals(SpellDataTargetType.Self))
            {
                if(unit.ChampionName.Equals("Kalista") && Player.Instance.Distance(unit) < 350)
                    CastW(castUnit);

                if (unit.ChampionName.Equals("Zed") && Player.Instance.Distance(unit) < 300)
                    CastW(castUnit);
            }
        }

        private static bool IsTargeted(this SpellDataTargetType type)
        {
            return type.Equals(SpellDataTargetType.Unit) || type.Equals(SpellDataTargetType.SelfAndUnit);
        }

        public static bool CastW(Obj_AI_Base target)
        {
            return target.IsValidTarget(SpellManager.W.Range) ? SpellManager.W.Cast(target) : SpellManager.W.Cast(target.ServerPosition);
        }

        static void OnDraw(EventArgs args)
        {

        }
    }
}
