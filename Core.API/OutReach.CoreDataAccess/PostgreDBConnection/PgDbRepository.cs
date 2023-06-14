using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace OutReach.CoreDataAccess.PostgreDBConnection
{
    public interface IDBRepository
    {
        #region ExecuteScalar
        Task<int> ExecuteNonQueryAsync(string commandText, List<DbParameter> parameters);
        Task<Object> ExecuteScalarAsync(string commandText, List<DbParameter> parameters);
        #endregion

        #region Execute ExecuteFirstOrDefaultAsync
        Task<T> ExecuteFirstOrDefaultAsync<T>(string sql, List<DbParameter> parameters);
        #endregion

        #region Execute List
        Task<List<T>> ExecuteListAsync<T>(string sql, List<DbParameter> parameters);
        #endregion
    }
    public class PgDbRepository : IDBRepository
    {
        public static string ConnectionString { get; set; }
        private IConfiguration _configuration;

        public PgDbRepository()
        {

        }
        public PgDbRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            if (_configuration != null &&
                !string.IsNullOrEmpty(_configuration.GetConnectionString("PGConnectionString")))
            {
                ConnectionString = _configuration.GetConnectionString("PGConnectionString");
            }
        }
       
        public static void SetPgConnection(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("PGConnectionString");
        }

        public static void SetPgConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #region ExecuteScalar

        public async Task<int> ExecuteNonQueryAsync(string commandText, List<DbParameter> parameters)
        {
            int result = 0;
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (DbParameter item in parameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }
                    await connection.OpenAsync();
                    try
                    {
                        result = await cmd.ExecuteNonQueryAsync();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result;
        }
        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, List<DbParameter> parameters)
        {
            int result = 0;
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (DbParameter item in parameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }
                    await connection.OpenAsync();
                    try
                    {
                        result = await cmd.ExecuteNonQueryAsync();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result;
        }
        #endregion
        
        #region ExecuteScalar
        public async Task<Object> ExecuteScalarAsync(string commandText, List<DbParameter> parameters)
        {
            object result = null;
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (DbParameter item in parameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }
                    await connection.OpenAsync();
                    try
                    {
                        result = await cmd.ExecuteScalarAsync();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result;
        }
        public async Task<Object> ExecuteScalarAsync(string commandText, CommandType commandType, List<DbParameter> parameters)
        {
            object result = null;
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (DbParameter item in parameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }
                    await connection.OpenAsync();
                    try
                    {
                        result = await cmd.ExecuteScalarAsync();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result;
        }
        #endregion

        #region Execute ExecuteFirstOrDefaultAsync
        public async Task<T> ExecuteFirstOrDefaultAsync<T>(string sql, List<DbParameter> parameters)
        {
            T result;
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    try
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.StoredProcedure;
                        var dynamicParameters = new DynamicParameters();
                        foreach (var item in parameters)
                        {
                            dynamicParameters.Add(item.ParameterName, item.Value);
                        }
                        CommandDefinition commandd = new CommandDefinition(sql, dynamicParameters, null, null, CommandType.StoredProcedure);
                        result = await connection.QueryFirstOrDefaultAsync<T>(commandd);
                        await connection.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result;
        }
        public async Task<T> ExecuteFirstOrDefaultAsync<T>(string sql, CommandType commandType, List<DbParameter> parameters)
        {
            T result;
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    try
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = commandType;
                        var dynamicParameters = new DynamicParameters();
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var item in parameters)
                            {
                                dynamicParameters.Add(item.ParameterName, item.Value);
                            }
                        }
                        CommandDefinition commandd = new CommandDefinition(sql, dynamicParameters, null, null, CommandType.StoredProcedure);
                        result = await connection.QueryFirstOrDefaultAsync<T>(commandd);
                        await connection.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result;
        }
        #endregion

        #region Execute List
        public async Task<List<T>> ExecuteListAsync<T>(string sql, List<DbParameter> parameters)
        {
            IEnumerable<T> result = null;
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    try
                    {
                        var dynamicParameters = new DynamicParameters();
                        foreach (var item in parameters)
                        {
                            dynamicParameters.Add(item.ParameterName, item.Value);
                        }
                        await connection.OpenAsync();
                        result = await connection.QueryAsync<T>(sql, dynamicParameters, null, null, CommandType.StoredProcedure);
                        await connection.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result.AsList();
        }
        public async Task<List<T>> ExecuteListAsync<T>(string sql, CommandType commandType, List<DbParameter> parameters)
        {
            IEnumerable<T> result = null;
            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    try
                    {
                        var dynamicParameters = new DynamicParameters();
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var item in parameters)
                            {
                                dynamicParameters.Add(item.ParameterName, item.Value);
                            }
                        }
                        await connection.OpenAsync();
                        result = await connection.QueryAsync<T>(sql, dynamicParameters, null, null, commandType);
                        await connection.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result.AsList();
        }
        
        #region  #region BuilderTool Changes
        public async Task<DataTable> ExecuteProcedures(string commandText, List<DbParameter> parameters)
        {
            DataTable dt = new DataTable();

            using (NpgsqlConnection connection = new NpgsqlConnection())
            {
                try
                {

                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = commandText;

                    #region Loop Through All Params
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                    #endregion

                    cmd.CommandType = CommandType.StoredProcedure;
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    using (NpgsqlDataReader sdr = cmd.ExecuteReader())
                    {
                        dt = new DataTable("DataTable1");
                        //Load DataReader into the DataTable.
                        dt.Load(sdr);
                    }
                    cmd.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    await connection.CloseAsync();
                }
                return dt;
            }

        }
        #endregion

        #endregion
    }
}
