using Dapper.Contrib.Extensions;

namespace TruckingSharp.Database.Entities
{
    [Table("bankaccounts")]
    public class PlayerBankAccount
    {
        [Key] public int Id { get; set; }

        public string Password { get; set; }
        public int Money { get; set; }
        public int PlayerId { get; set; }
    }
}