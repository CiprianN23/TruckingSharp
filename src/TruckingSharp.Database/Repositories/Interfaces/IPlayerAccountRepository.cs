using System.Collections.Generic;
using TruckingSharp.Database.Entities;

namespace TruckingSharp.Database.Repositories.Interfaces
{
    public interface IPlayerAccountRepository : IRepository<PlayerAccount>
    {
        IEnumerable<PlayerAccount> GetBestPlayers(int amountOfPlayers);
    }
}