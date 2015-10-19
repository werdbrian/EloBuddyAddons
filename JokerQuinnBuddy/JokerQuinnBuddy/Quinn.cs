using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

using ComboSettings = JokerQuinnBuddy.Config.Modes.Combo;

namespace JokerQuinnBuddy
{
    public static class Quinn
    {
        public static uint qRange = SpellManager.Q.Range;
        public static uint eRange = SpellManager.E.Range;

        static Quinn()
        {
            GameObject.OnCreate += OnCreate;

            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
            Gapcloser.OnGapcloser += OnGapcloser;

            Obj_AI_Base.OnProcessSpellCast += EReset;
            QuinnRanges();

            Orbwalker.OnPostAttack += OnPostAttack;
        }

        public static void eLogic()
        {
            var target = TargetSelector2.GetTarget(qRange, DamageType.Physical);

            if (target == null || !target.IsValidTarget())
                return;

            if (target.HasBuff("QuinnW"))
                Player.IssueOrder(GameObjectOrder.AutoAttack, target);

            if (target.HasBuff("QuinnW"))
                return;

            foreach (var minion in EntityManager.MinionsAndMonsters.Minions.Where(minion => minion.IsValidTarget() && minion.IsEnemy &&
                minion.Distance(Player.Instance.ServerPosition) <= eRange))
            {
                if (Player.Instance.HasBuff("quinnrtimeout") || Player.Instance.HasBuff("QuinnRForm"))
                    return;

                var eCastPos = Player.Instance.ServerPosition.Extend(minion.Position, 
                    Player.Instance.ServerPosition.Distance(minion.Position) - (Player.Instance.GetAutoAttackRange() - Player.Instance.Distance(minion.Position)));

                if (eCastPos.Distance(target.Position) < Player.Instance.GetAutoAttackRange()
                    && target.Distance(Player.Instance.Position) > Player.Instance.GetAutoAttackRange() && target.Health < CalcDamage(target) && !Player.Instance.IsFacing(minion))
                    SpellManager.E.Cast(minion);

            }

            if (SpellManager.E.IsReady())
                SpellManager.E.Cast(target);
        }

        public static void wLogic()
        {
            if (SpellManager.W.IsReady() && AnyEnemyInBush())
                SpellManager.W.Cast();
        }

        public static void rLogic()
        {
            var target = TargetSelector2.GetTarget(eRange, DamageType.Physical);

            if (target == null || !target.IsValidTarget())
                return;

            if (Player.HasBuff("quinnrtimeout") || Player.HasBuff("QuinnRForm"))
                ASMode();

            if (Player.HasBuff("quinnrtimeout") || Player.HasBuff("QuinnRForm"))
                return;

            if (SpellManager.E.IsReady() && SpellManager.R.IsReady() && Player.Instance.Distance(target) < eRange &&
                target.Health < DiveDmgCalc(target))
                SpellManager.R.Cast();

            if (SpellManager.E.IsReady() && SpellManager.R.IsReady() && Player.Instance.Distance(target) < eRange &&
                target.Health < CalcDamage(target) * 1.2)
                SpellManager.R.Cast();
        }

        public static void ASMode()
        {
            var target = TargetSelector2.GetTarget(1200, DamageType.Physical);

            if (target == null || !target.IsValidTarget())
                return;

            var ultFinisher = Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)(75 + (SpellManager.R.Level * 55) + (Player.Instance.FlatPhysicalDamageMod * 0.5)) * (2 - (target.Health / target.MaxHealth)));

            var ultIgnite = IgniteDamage(target) + Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)(75 + (SpellManager.R.Level * 55) + (Player.Instance.FlatPhysicalDamageMod * 0.5)) * (2 - (target.Health / target.MaxHealth)));

            if(ComboSettings.UseCutlassBOTRK)
                ItemManager.UseCastables();

            if (target.IsValidTarget(900) && ComboSettings.UseYomuus)
                ItemManager.UseYomu();

            if (SpellManager.E.IsReady() && ComboSettings.UseE)
                SpellManager.E.Cast(target);

            if (SpellManager.Q.IsReady() && Player.Instance.Position.CountEnemiesInRange(250) > 0 && ComboSettings.UseQ)
                SpellManager.Q.Cast(target);

            if (SpellManager.R.IsReady() && SpellManager.IG.IsReady() && ultIgnite > target.Health && Player.Instance.Position.CountEnemiesInRange(500) > 0 && ComboSettings.UseR)
                SpellManager.R.Cast();

            if (SpellManager.R.IsReady() && SpellManager.IG.IsReady() && ultIgnite + Player.Instance.GetAutoAttackDamage(target) > target.Health &&
                Player.Instance.Position.CountEnemiesInRange(500) > 0 && !SpellManager.E.IsReady() && ComboSettings.UseR)
                SpellManager.R.Cast();

            if (SpellManager.R.IsReady() && ultFinisher > target.Health && Player.Instance.Position.CountEnemiesInRange(500) > 0 && ComboSettings.UseR)
                SpellManager.R.Cast();

            if (SpellManager.R.IsReady() && ultFinisher + Player.Instance.GetAutoAttackDamage(target) > target.Health && Player.Instance.Position.CountEnemiesInRange(500) > 0
                && target.Distance(Player.Instance.Position) > Player.Instance.GetAutoAttackRange() && !SpellManager.E.IsReady() && ComboSettings.UseR)
                SpellManager.R.Cast();
        }

        public static float DiveDmgCalc(Obj_AI_Base target)
        {
            var aa = Player.Instance.GetAutoAttackDamage(target, true) * (1 + Player.Instance.Crit);
            double damage = 0;

            if (SpellManager.IG.IsReady())
                damage += Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);

            if (ItemManager.BOTRK.IsOwned() && ItemManager.BOTRK.IsReady())
                damage += Player.Instance.GetItemDamage(target, ItemId.Blade_of_the_Ruined_King);

            if (ItemManager.Cutl.IsOwned() && ItemManager.Cutl.IsReady())
                damage += Player.Instance.GetItemDamage(target, ItemId.Bilgewater_Cutlass);

            var maxHealth = target.MaxHealth;
            var Health = target.Health - damage;

            double dmgAfterR = Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                (float)((75 + (SpellManager.R.Level * 55) + (Player.Instance.FlatPhysicalDamageMod * 0.5)) * (2 - (Health / maxHealth))));

            if (SpellManager.R.IsReady())
                damage += dmgAfterR + aa;

            return (float)damage;
        }

        public static float CalcDamage(Obj_AI_Base target)
        {
            var aa = Player.Instance.GetAutoAttackDamage(target, true) * (1 + Player.Instance.Crit);
            double damage = aa;

            if (SpellManager.IG.IsReady())
                damage += Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);

            if (ItemManager.BOTRK.IsOwned() && ItemManager.BOTRK.IsReady())
                damage += Player.Instance.GetItemDamage(target, ItemId.Blade_of_the_Ruined_King);

            if (ItemManager.Cutl.IsOwned() && ItemManager.Cutl.IsReady())
                damage += Player.Instance.GetItemDamage(target, ItemId.Bilgewater_Cutlass);

            if (SpellManager.E.IsReady())
                damage += Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float)(10 + (SpellManager.E.Level * 30) +
                    (Player.Instance.FlatPhysicalDamageMod * 0.3) + aa * 2));

            if (SpellManager.Q.IsReady())
                damage += Player.Instance.GetSpellDamage(target, SpellSlot.Q);

            if (target.HasBuff("QuinnW") && !SpellManager.E.IsReady())
                damage += Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, (float)(15 + (Player.Instance.Level * 10)
                    + (Player.Instance.FlatPhysicalDamageMod * 0.5)));

            if (SpellManager.R.IsReady())
                damage += SpellManager.R.Level * 125 + aa;

            return (float)damage;
        }

        public static void Initialize()
        {

        }

        private static void QuinnRanges()
        {
            if (Player.Instance.HasBuff("quinnrtimeout") || Player.Instance.HasBuff("QuinnRForm"))
                qRange = 200;

            eRange = 650;
        }

        private static float IgniteDamage(AIHeroClient target)
        {
            if (SpellManager.IG.Slot == SpellSlot.Unknown || Player.Instance.Spellbook.CanUseSpell(SpellManager.IG.Slot) != SpellState.Ready)
                return 0f;

            return (float)Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);
        }

        private static bool AnyEnemyInBush()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                if (enemy.IsValid && enemy.IsVisible && !enemy.IsDead)
                {
                    if (NavMesh.IsWallOfGrass(enemy.ServerPosition, 10) &&
                        ObjectManager.Player.ServerPosition.Distance(enemy.ServerPosition) < 650)

                        return true;
                }
            }

            return false;
        }

        private static void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            var eTarget = TargetSelector2.GetTarget(eRange, DamageType.Physical);
            if(SpellManager.E.IsReady() && eTarget.IsValidTarget() && !target.IsStructure() && JokerQuinnBuddy.Config.Modes.Perma.UseEAfterAA)
                SpellManager.E.Cast(eTarget);
        }

        private static void EReset(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "QuinnE")
                Orbwalker.ResetAutoAttack();
        }

        private static void OnCreate(GameObject sender, EventArgs args)
        {
            var rengar = EntityManager.Heroes.Enemies.Find(h => h.ChampionName.Equals("Rengar"));

            if (rengar != null)

                if (sender.Name == "Rengar_LeapSound.troy" && sender.Position.Distance(Player.Instance.Position) < eRange && SpellManager.E.IsReady())
                    SpellManager.E.Cast(rengar);

            var khazix = EntityManager.Heroes.Enemies.Find(h => h.ChampionName.Equals("Khazix"));

            if (khazix != null)

                if (sender.Name == "Khazix_Base_E_Tar.troy" && sender.Position.Distance(Player.Instance.Position) < eRange && SpellManager.E.IsReady())
                    SpellManager.E.Cast(khazix);
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (SpellManager.E.IsReady() && sender.IsValidTarget(eRange))
                SpellManager.E.Cast(sender);
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (SpellManager.E.IsReady() && sender.IsValidTarget(eRange))
                SpellManager.E.Cast(e.Sender);
        }

    }
}
