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
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }
        public static Spell.Targeted IG { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1010, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W, 1500);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R, 550);

            if (!(ObjectManager.Player.GetSpellSlotFromName("summonerdot") == SpellSlot.Unknown))
                IG = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 550);
        }

        public static void Initialize()
        {

        }
    }
}
