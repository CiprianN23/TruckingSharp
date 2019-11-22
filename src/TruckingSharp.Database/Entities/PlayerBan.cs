using Dapper.Contrib.Extensions;
using System;

namespace TruckingSharp.Database.Entities
{
    public class PlayerBan
    {
        [Key]
        public int Id { get; set; }

        public string Reason { get; set; }
        public DateTime Duration { get; set; }
        public int AdminId { get; set; }
        public int OwnerId { get; set; }
    }
}