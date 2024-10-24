using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace CS2_SimpleAdmin.Database;

public class Database(string dbConnectionString)
{
    private CS2_SimpleAdminConfig? _config;

    public MySqlConnection GetConnection()
    {
        try
        {
            var connection = new MySqlConnection(dbConnectionString);
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            CS2_SimpleAdmin._logger?.LogCritical($"Unable to connect to database: {ex.Message}");
            throw;
        }
    }

    public SqliteConnection GetSQLiteConnection()
    {
        try
        {
            var connection = new SqliteConnection(dbConnectionString);
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            CS2_SimpleAdmin._logger?.LogCritical($"Unable to connect to database: {ex.Message}");
            throw;
        }
    }

    public async Task<MySqlConnection> GetConnectionAsync()
    {
        try
        {
            var connection = new MySqlConnection(dbConnectionString);
            await connection.OpenAsync();
            return connection;
        }
        catch (Exception ex)
        {
            CS2_SimpleAdmin._logger?.LogCritical($"Unable to connect to database: {ex.Message}");
            throw;
        }
    }

    public async Task<SqliteConnection> GetSQLiteConnectionAsync()
    {
        try
        {
            var connection = new SqliteConnection(dbConnectionString);
            await connection.OpenAsync();
            return connection;
        }
        catch (Exception ex)
        {
            CS2_SimpleAdmin._logger?.LogCritical($"Unable to connect to database: {ex.Message}");
            throw;
        }
    }

    public void DatabaseMigration()
    {
        Migration migrator = new(this);
        migrator.ExecuteMigrations();
    }

    public bool CheckDatabaseConnection(bool sqlite)
    {
        if (sqlite)
        {
            using var connection = GetConnection();

            try
            {
                return connection.Ping();
            }
            catch
            {
                return false;
            }
        }
        else
        {
            using var connection = GetSQLiteConnection();

            try
            {
                return connection.State == System.Data.ConnectionState.Open ? true : false;
            }
            catch
            {
                return false;
            }
        }
    }
}