using Dapper.Contrib.Extensions;
using System;

namespace TruckingSharp.Database.Entities
{
    [Table("accounts")]
    public class PlayerAccount
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }
        public int Money { get; set; }
        public int Score { get; set; }
        public byte AdminLevel { get; set; }
        public byte RulesRead { get; set; }
        public DateTime Muted { get; set; }
        public int Jailed { get; set; }
        public int Wanted { get; set; }
        public int Bans { get; set; }
        public DateTime BanTime { get; set; }
        public byte TruckerLicense { get; set; }
        public byte BusLicense { get; set; }
        public float MetersDriven { get; set; }
        public int TruckerJobs { get; set; }
        public int ConvoyJobs { get; set; }
        public int BusDriverJobs { get; set; }
        public int PilotJobs { get; set; }
        public int MafiaJobs { get; set; }
        public int MafiaStolen { get; set; }
        public int PoliceFined { get; set; }
        public int PoliceJailed { get; set; }
        public int AssistanceJobs { get; set; }
        public int CourierJobs { get; set; }
        public int RoadWorkerJobs { get; set; }
    }
}