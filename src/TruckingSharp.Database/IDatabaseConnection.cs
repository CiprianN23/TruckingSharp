using System.Data;
using System.Threading.Tasks;

namespace TruckingSharp.Database
{
    public interface IDatabaseConnection
    {
        IDbConnection CreateConnection();

        Task<IDbConnection> CreateConnectionAsync();
    }
}