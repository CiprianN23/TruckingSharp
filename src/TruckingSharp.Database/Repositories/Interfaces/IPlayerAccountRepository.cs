using TruckingSharp.Database.Entities;
using System.Collections.Generic;

namespace TruckingSharp.Database.Repositories.Interfaces
{
    public interface IPlayerAccountRepository : IRepository<PlayerAccount>
    {
        IEnumerable<PlayerAccount> GetBestPlayers(int amountOfPlayers);
    }
}
