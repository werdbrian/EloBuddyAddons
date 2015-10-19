using System.Linq;
using System;
using EloBuddy;
using EloBuddy.SDK;

using Settings = JokerQuinnBuddy.Config.Modes.Perma;
namespace JokerQuinnBuddy.Modes
{
    public sealed class PermaActive : ModeBase
    {

        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            if (ObjectManager.Player.IsDead || !SpellManager.IG.IsReady() || !Settings.UseIgnite) return;
            if (ObjectManager.Get<AIHeroClient>().Where(
                        h =>
                            h.IsValidTarget(SpellManager.IG.Range) &&
                            h.Health <
                            ObjectManager.Player.GetSummonerSpellDamage(h, DamageLibrary.SummonerSpells.Ignite)).Count() <= 0) return;

            var target = ObjectManager.Get<AIHeroClient>()
                .Where(
                        h =>
                            h.IsValidTarget(SpellManager.IG.Range) &&
                            h.Health <
                            ObjectManager.Player.GetSummonerSpellDamage(h, DamageLibrary.SummonerSpells.Ignite));

            SpellManager.IG.Cast(target.First());
        }
    }
}
