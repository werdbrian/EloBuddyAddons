using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using EloBuddy.SDK;

namespace JokerFioraBuddy
{
    public static class PassiveManager
    {
        public static List<FioraPassive> PassiveList = new List<FioraPassive>();
        private static readonly List<string> DirectionList = new List<string> { "NE", "NW", "SE", "SW" };

        static PassiveManager()
        {
            GameObject.OnCreate += OnCreate;
            GameObject.OnDelete += OnDelete;
        }

        public static int CountPassive(this Obj_AI_Base target)
        {
            return PassiveList.Count(obj => obj.Position.Distance(target.ServerPosition) <= 50);
        }

        public static FioraPassive GetNearestPassive(this Obj_AI_Base target)
        {
            return
                PassiveList.OrderBy(obj => obj.Position.Distance(target.ServerPosition)).FirstOrDefault(obj => obj.IsValid);
        }

        public static Vector3 GetPassivePosition(this Obj_AI_Base target)
        {
            var passive = target.GetNearestPassive();

            if (passive == null || target.Distance(passive.Position) == 0)
                return Vector3.Zero;

            var pos = Prediction.Position.PredictUnitPosition(target, SpellManager.Q.CastDelay);
            var d = passive.PassiveDistance;

            if (passive.Name.Contains("NE"))
                pos.Y += d;

            if (passive.Name.Contains("SE"))
                pos.X -= d;

            if (passive.Name.Contains("NW"))
                pos.X += d;

            if (passive.Name.Contains("SW"))
                pos.Y -= d;

            return pos.To3D();
        }

        public static double GetPassiveDamage(this Obj_AI_Base target)
        {
            var count = target.CountPassive();
            return count == 0 ? 0 : GetPassiveDamage(target, count);
        }

        public static double GetPassiveDamage(this Obj_AI_Base target, int passiveCount)
        {
            return passiveCount * (.03f + Math.Min(Math.Max(.028f, (.027 + .001f * ObjectManager.Player.Level * ObjectManager.Player.FlatPhysicalDamageMod / 100f)), .45f)) * target.MaxHealth;
        }

        static void OnCreate(GameObject sender, EventArgs args)
        {
            var emitter = sender as Obj_GeneralParticleEmitter;

            if (emitter == null || !emitter.IsValid)
                return;

            if (emitter.Name.Contains("Fiora_Base_Passive") && DirectionList.Any(emitter.Name.Contains) && !emitter.Name.Contains("Warning"))
                PassiveList.Add(new FioraPassive(emitter));

            if (emitter.Name.Contains("Fiora_Base_R_Mark") || (emitter.Name.Contains("Fiora_Base_R") && emitter.Name.Contains("Timeout")))
                PassiveList.Add(new FioraPassive(emitter, true));
        }

        static void OnDelete(GameObject sender, EventArgs args)
        {
            if (sender.Name.Contains("Fiora_Base_Passive") && DirectionList.Any(sender.Name.Contains) && !sender.Name.Contains("Warning"))
                PassiveList.RemoveAll(obj => obj.NetworkId.Equals(sender.NetworkId));

            else if (sender.Name.Contains("Fiora_Base_R_Mark") || (sender.Name.Contains("Fiora_Base_R") && sender.Name.Contains("Timeout")))
                PassiveList.RemoveAll(obj => obj.NetworkId.Equals(sender.NetworkId));
        }

        public static void Initialize()
        {

        }


        public class FioraPassive : Obj_GeneralParticleEmitter
        {
            private readonly int MaxAliveTime;
            private readonly int SpawnTime;
            public bool IsUltPassive;
            public int PassiveDistance;

            public FioraPassive(Obj_GeneralParticleEmitter emitter, bool ultPassive = false)
            {
                SpawnTime = Game.TicksPerSecond;
                IsUltPassive = ultPassive;
                MaxAliveTime = IsUltPassive ? 8000 : 15000;
                PassiveDistance = IsUltPassive ? 320 : 200;
            }

            private int VitalDuration
            {
                get { return Game.TicksPerSecond - SpawnTime; }
            }

            private bool IsVitalIdentified
            {
                get { return VitalDuration > 500; }
            }

            private bool IsVitalTimedOut
            {
                get { return VitalDuration < MaxAliveTime; }
            }

            public bool IsActive
            {
                get { return IsValid && IsVisible && !IsVitalTimedOut; }
            }
        }
    }
}
