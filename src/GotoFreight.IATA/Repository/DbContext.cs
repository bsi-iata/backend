using System.Data;
using Npgsql;
using MySql.Data.MySqlClient;

namespace GotoFreight.IATA.Repository;

public class DbContext
{
    private readonly IConfiguration _configuration;
    private readonly NpgsqlDataSource _npgsqlDataSource;

    // ReSharper disable once ConvertToPrimaryConstructor
    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;

        var connStr = _configuration.GetValue<string>("ConnectionStrings:Postgres");
        if (!string.IsNullOrWhiteSpace(connStr))
        {
            _npgsqlDataSource = NpgsqlDataSource.Create(connStr);
        }
    }

    public async Task<IDbConnection> GetConnection()
    {
        var connStr = _configuration.GetValue<string>("ConnectionStrings:MySql");
        if (!string.IsNullOrWhiteSpace(connStr))
        {
            return new MySqlConnection(connStr);
        }

        return _npgsqlDataSource != null ? await _npgsqlDataSource.OpenConnectionAsync() : null;
    }
}