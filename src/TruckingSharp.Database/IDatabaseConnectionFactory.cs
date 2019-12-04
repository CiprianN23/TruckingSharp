using System.Data;
using System.Threading.Tasks;

namespace TruckingSharp.Database
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();

        Task<IDbConnection> CreateConnectionAsync();
    }
}