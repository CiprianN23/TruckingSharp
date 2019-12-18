using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using TruckingSharp.Database.Entities;

namespace TruckingSharp.Database.Repositories
{
    public class ClassSpawnRepository
    {
        private readonly IDatabaseConnection _databaseConnectionFactory;

        public ClassSpawnRepository(IDatabaseConnection databaseConnectionFactory) => _databaseConnectionFactory = databaseConnectionFactory;

        public IEnumerable<ClassSpawn> GetAllByClassType(int classType)
        {
            try
            {
                const string command = "SELECT * FROM classspawns WHERE class_type = @type;";

                using (var sqlConnection = _databaseConnectionFactory.CreateConnection())
                {
                    return sqlConnection.Query<ClassSpawn>(command, new { type = classType });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to fetch all class spawn of type {classType}.");
                throw;
            }
        }
    }
}