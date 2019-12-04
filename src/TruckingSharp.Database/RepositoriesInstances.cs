using TruckingSharp.Database.Repositories;

namespace TruckingSharp.Database
{
    public static class RepositoriesInstances
    {
        public static PlayerAccountRepository AccountRepository => new PlayerAccountRepository(new PostgresConnectionFactory());
        public static PlayerBankAccountRepository PlayerBankAccountRepository => new PlayerBankAccountRepository(new PostgresConnectionFactory());
        public static PlayerBanRepository PlayerBanRepository => new PlayerBanRepository(new PostgresConnectionFactory());
        public static SpeedCameraRepository SpeedCameraRepository => new SpeedCameraRepository(new PostgresConnectionFactory());
    }
}