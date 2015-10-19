using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Events;
using SharpDX;
using JokerQuinnBuddy.Modes;

using Settings = JokerQuinnBuddy.Config.Drawings;
namespace JokerQuinnBuddy
{
    public static class TargetSelector2
    {
        private static AIHeroClient _target;

        static TargetSelector2()
        {
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        public static AIHeroClient GetTarget(float range, DamageType type, Vector2 secondaryPos = new Vector2())
        {
            if (_target == null || _target.IsDead || _target.Health <= 0 || !_target.IsValidTarget())
                _target = null;

            if (secondaryPos.IsValid() && _target.Distance(secondaryPos) < range || _target.IsValidTarget(range))
                return _target;

            return TargetSelector.GetTarget(range, type);
        }

        static void OnDraw(EventArgs args)
        {
            if (_target != null && Settings.ShowChampionTarget && !_target.IsDead)
                Circle.Draw(Color.Cyan, 100, _target.Position);
        }

        static void OnUpdate(EventArgs args)
        {
            _target = ObjectManager.Get<AIHeroClient>().OrderBy(a => a.Distance(ObjectManager.Player)).FirstOrDefault(a => a.IsEnemy && a.Distance(ObjectManager.Player) <= Player.Instance.GetAutoAttackRange());
        }

        public static void Initialize()
        {

        }
    }
}
