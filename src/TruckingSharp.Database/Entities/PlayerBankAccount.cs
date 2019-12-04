namespace TruckingSharp.Database.Entities
{
    public class PlayerBankAccount
    {
        public int Id { get; set; }

        public string Password { get; set; }
        public int Money { get; set; }
        public int PlayerId { get; set; }
    }
}