using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

namespace JokerFioraBuddy
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Targeted R { get; private set; }
        public static Spell.Targeted IG { get; private set; }

        static SpellManager()
        {  
            Q = new Spell.Skillshot(SpellSlot.Q, 600, SkillShotType.Circular, 250, int.MaxValue);
            W = new Spell.Skillshot(SpellSlot.W, 750, SkillShotType.Linear, 250, int.MaxValue);
            E = new Spell.Active(SpellSlot.E, 200);
            R = new Spell.Targeted(SpellSlot.R, 550);

            if (ObjectManager.Player.GetSpellSlotFromName("summonerdot") == SpellSlot.Unknown)
            {
                return;
            }

            IG = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 550);


        }

        public static void Initialize()
        { 

        }
    }
}
