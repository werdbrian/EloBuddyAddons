using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JokerFioraBuddy
{
    public static class SpellBlock
    {
        const SpellSlot q = SpellSlot.Q;
        const SpellSlot w = SpellSlot.W;
        const SpellSlot e = SpellSlot.E;
        const SpellSlot r = SpellSlot.R;
        const SpellSlot N48 = (SpellSlot)48;

        static SpellBlock()
        {
            new BlockedSpell("Akali", q).Add();
            new BlockedSpell("Anivia", e).Add();
            new BlockedSpell("Annie", q).Add();
            new BlockedSpell("Alistar", w).Add();
            new BlockedSpell("Azir", r).Add();
            new BlockedSpell("Bard", r).Add();
            new BlockedSpell("Blitzcrank", e) { AutoAttackName = "PowerFistAttack" }.Add();
            new BlockedSpell("Brand", r).Add();

            new BlockedSpell("Chogath", r).Add();
            new BlockedSpell("Darius", r).Add();
            new BlockedSpell("Fiddlesticks", q).Add();
            new BlockedSpell("Fizz", q).Add();
            new BlockedSpell("Gangplank", q).Add();
            new BlockedSpell("Garen", q) { AutoAttackName = "GarenQAttack" }.Add();
            new BlockedSpell("Garen", r).Add();
            new BlockedSpell("Gragas", w) { AutoAttackName = "DrunkenRage" }.Add();
            new BlockedSpell("Hecarim", e) { AutoAttackName = "hecarimrampattack" }.Add();
            new BlockedSpell("Irelia", e).Add();
            new BlockedSpell("Jayce", e).Add();
            new BlockedSpell("Kassadin", q).Add();
            new BlockedSpell("Khazix", q).Add();
            new BlockedSpell("LeBlanc", r).Add();
            new BlockedSpell("LeeSin", r).Add();
            new BlockedSpell("Leona", q) { AutoAttackName = "LeonaShieldOfDaybreakAttack" }.Add();
            new BlockedSpell("Lissandra", N48).Add();
            new BlockedSpell("Lulu", w).Add();
            new BlockedSpell("Malphite", q).Add();
            new BlockedSpell("Maokai", w).Add();
            new BlockedSpell("MissFortune", q).Add();
            new BlockedSpell("MonkeyKing", q) { AutoAttackName = "MonkeyKingQAttack" }.Add();
            new BlockedSpell("Mordekaiser", q) { AutoAttackName = "mordekaiserqattack2" }.Add();
            new BlockedSpell("Mordekaiser", r).Add();
            new BlockedSpell("Nasus", q) { AutoAttackName = "NasusQAttack" }.Add();
            new BlockedSpell("Nasus", w).Add();
            new BlockedSpell("Nidalee", q) { AutoAttackName = "NidaleeTakedownAttack", ModelName = "nidalee_cougar" }.Add();
            new BlockedSpell("Nunu", e).Add();
            new BlockedSpell("Malzahar", r).Add();
            new BlockedSpell("Pantheon", w).Add();
            new BlockedSpell("Poppy", q) { AutoAttackBuff = "PoppyDevastatingBlow" }.Add();
            new BlockedSpell("Poppy", e).Add();
            new BlockedSpell("Quinn", e) { ModelName = "quinnvalor" }.Add();
            new BlockedSpell("Rammus", e).Add();
            new BlockedSpell("Renekton", w) { AutoAttackName = "RenektonExecute" }.Add();
            new BlockedSpell("Renekton", w) { AutoAttackName = "RenektonSuperExecute" }.Add();
            new BlockedSpell("Rengar", q) { AutoAttackName = "RengarBasicAttack" }.Add();
            new BlockedSpell("Riven", q).Add();
            new BlockedSpell("Ryze", w).Add();
            new BlockedSpell("Shaco", q).Add();
            new BlockedSpell("Shyvana", q) { AutoAttackName = "ShyvanaDoubleAttackHit" }.Add();
            new BlockedSpell("Singed", e).Add();
            new BlockedSpell("Skarner", r).Add();
            new BlockedSpell("Syndra", r).Add();
            new BlockedSpell("Swain", e).Add();
            new BlockedSpell("TahmKench", w).Add();
            new BlockedSpell("Talon", e).Add();
            new BlockedSpell("Taric", e).Add();
            new BlockedSpell("Teemo", q).Add();
            new BlockedSpell("Tristana", e, false).Add();
            new BlockedSpell("Tristana", r).Add();
            new BlockedSpell("Trundle", q) { AutoAttackName = "TrundleQ" }.Add();
            new BlockedSpell("Trundle", r).Add();
            new BlockedSpell("TwistedFate", w) { AutoAttackName = "goldcardpreattack" }.Add();
            new BlockedSpell("Udyr", e) { AutoAttackName = "UdyrBearAttack" }.Add();
            new BlockedSpell("Urgot", r).Add();
            new BlockedSpell("Vayne", e).Add();
            new BlockedSpell("Veigar", r).Add();
            new BlockedSpell("Vi", r).Add();
            new BlockedSpell("Vladimir", r).Add();
            new BlockedSpell("Volibear", q) { AutoAttackName = "VolibearQAttack" }.Add();
            new BlockedSpell("Volibear", w).Add();
            new BlockedSpell("XinZhao", q) { AutoAttackName = "XenZhaoThrust3" }.Add();
            new BlockedSpell("XinZhao", r).Add();
            new BlockedSpell("Yorick", e).Add();
            new BlockedSpell("Zac", r).Add();
        }

        public static bool Contains(AIHeroClient unit, GameObjectProcessSpellCastEventArgs args)
        {
            var name = unit.ChampionName;
            var spellSlot = unit.GetSpellSlotFromName(args.SData.Name);
            var slot = spellSlot.Equals(48) ? SpellSlot.R : spellSlot;

            if (args.SData.Name.Equals("KalistaRAllyDash"))
                return true;

            foreach (var spell in BlockedSpell.GetBlockedSpells().Where(o => o.Name.Equals(name)).Where(spell => !spell.HasModelCondition || unit.BaseSkinName.Equals(spell.ModelName)).Where(spell => !spell.HasBuffCondition || unit.HasBuff(spell.AutoAttackBuff)))
            {
                if (spell.IsAutoAttack)
                {
                    if (!args.SData.ConsideredAsAutoAttack)
                        continue;

                    var condition = spell.AutoAttackName.Equals(args.SData.Name);

                    if (unit.ChampionName.Equals("Rengar"))
                        condition = condition && unit.Mana.Equals(5);

                    if (condition)
                        return true;

                    continue;
                }

                if (!spell.Slot.Equals(slot))
                    continue;

                if (name.Equals("Riven"))
                {
                    var buff = unit.Buffs.FirstOrDefault(b => b.Name.Equals("RivenTriCleave"));

                    if (buff != null && buff.Count == 3)
                        return true;
                }

                return true;
            }
            return false;
        }

        public static void Initialize()
        {
 
        }
    }

    public class BlockedSpell
    {
        private static readonly List<BlockedSpell> blockedSpells = new List<BlockedSpell>();
        public string AutoAttackBuff;
        public string AutoAttackName;
        public bool Enabled;
        public bool IsSelfBuff;
        public string ModelName;
        public string Name;
        public SpellSlot Slot;

        public BlockedSpell(string name, SpellSlot slot, bool enabled = true)
        {
            Name = name;
            Slot = slot;
            Enabled = enabled;
        }

        public bool IsAutoAttack
        {
            get { return !string.IsNullOrWhiteSpace(AutoAttackName); }
        }

        public bool HasBuffCondition
        {
            get { return !string.IsNullOrWhiteSpace(AutoAttackBuff); }
        }

        public bool HasModelCondition
        {
            get { return !string.IsNullOrWhiteSpace(ModelName); }
        }

        public static List<BlockedSpell> GetBlockedSpells()
        {
            return blockedSpells;
        }

        public void Add()
        {
            blockedSpells.Add(this);
        }

    }
}
