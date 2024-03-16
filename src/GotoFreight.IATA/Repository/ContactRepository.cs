using Dapper;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Repository;

public class ContactRepository
{
    private readonly DbContext _dbContext;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ContactRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> Upsert(Contact contact)
    {
        var sql = """
                  insert into contacts(Name, Phone, Email, Contry, State, City, Zip, Address, CreateTime, UpdateTime)
                      value (@Name, @Phone, @Email, @Contry, @State, @City, @Zip, @Address, @CreateTime, @UpdateTime)
                  ON DUPLICATE KEY UPDATE Name       = @Name,
                                          Phone      = @Phone,
                                          Email      = @Email,
                                          Contry     = @Contry,
                                          State      = @State,
                                          City       = @City,
                                          Zip        = @Zip,
                                          Address    = @Address,
                                          UpdateTime = @UpdateTime;
                  SELECT LAST_INSERT_ID();
                  """;

        using var conn = await _dbContext.GetConnection();
        return await conn.QuerySingleAsync<long>(sql, contact);
    }
}