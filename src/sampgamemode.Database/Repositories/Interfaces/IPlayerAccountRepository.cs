using sampgamemode.Database.Entities;
using System.Collections.Generic;

namespace sampgamemode.Database.Repositories.Interfaces
{
    public interface IPlayerAccountRepository : IRepository<PlayerAccount>
    {
        IEnumerable<PlayerAccount> GetBestPlayers(int amountOfPlayers);
    }
}
