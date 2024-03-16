using Dapper;
using GotoFreight.IATA.Models.Dto;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Repository;

public class OrderRepository
{
    private readonly DbContext _dbContext;

    // ReSharper disable once ConvertToPrimaryConstructor
    public OrderRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(Order order)
    {
        using var conn = await _dbContext.GetConnection();
        conn.Open();
        using var tran = conn.BeginTransaction();
        try
        {
            //保存订单信息
            var sql = """
                      insert into orders(Code, UserId, Airline, Flight, DepartureAddress, ArrivalAddress, Departure, Arrival, Status, Reference, Remark, CreateTime, UpdateTime)
                      value (@Code, @UserId, @Airline, @Flight, @DepartureAddress, @ArrivalAddress, @Departure, @Arrival, @Status, @Reference, @Remark, @CreateTime, @UpdateTime)
                      """;
            await conn.ExecuteAsync(sql, order);

            //保存包裹信息
            sql = """
                  insert into packages(OrderCode, ContactId, Weight, Volumn, Quantity, Remark, GoodsDesc, CreateTime, UpdateTime)
                  value (@OrderCode, @ContactId, @Weight, @Volumn, @Quantity, @Remark, @GoodsDesc, @CreateTime, @UpdateTime);
                  SELECT LAST_INSERT_ID();
                  """;
            var packageId = await conn.QuerySingleAsync<long>(sql, order.Packages.FirstOrDefault());
            foreach (var good in order.Packages.FirstOrDefault().Goods)
            {
                good.PackageId = packageId;
            }

            //保存货物信息
            sql = """
                  insert into goods(PackageId, Commodity, Pcs, Price, Amount, HsCode, `Usage`, Materia, Orginal, Photo, AIResult,
                                    CreateTime, UpdateTime)
                  values (@PackageId, @Commodity, @Pcs, @Price, @Amount, @HsCode, @Usage, @Materia, @Orginal, @Photo, @AIResult,
                          @CreateTime, @UpdateTime)
                  """;
            await conn.ExecuteAsync(sql, order.Packages.FirstOrDefault().Goods);

            //提交事务
            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task Remove(string code)
    {
        using var conn = await _dbContext.GetConnection();
        conn.Open();
        using var tran = conn.BeginTransaction();
        try
        {
            var sql = "select * from packages where OrderCode = @OrderCode";
            var packages = await conn.QueryAsync<Package>(sql, new { OrderCode = code });

            sql = "delete from orders where Code = @Code";
            await conn.ExecuteAsync(sql, new { Code = code });

            sql = "delete from packages where OrderCode = @OrderCode";
            await conn.ExecuteAsync(sql, new { OrderCode = code });

            sql = "delete from goods where PackageId in @PackageIds";
            await conn.ExecuteAsync(sql, new { PackageIds = packages.Select(q => q.Id).ToList() });

            tran.Commit();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            throw;
        }
    }

    public async Task<Order> Find(string code)
    {
        using var conn = await _dbContext.GetConnection();

        var sql = "select * from orders where Code = @Code";
        var order = await conn.QueryFirstOrDefaultAsync<Order>(sql, new { Code = code });
        if (order == null)
        {
            throw new Exception("order not found");
        }

        sql = "select * from packages where OrderCode = @OrderCode";
        var packages = await conn.QueryAsync<Package>(sql, new { OrderCode = order.Code });
        order.Packages = packages.Where(q => q.OrderCode == order.Code).ToList();

        sql = "select * from contacts where Id in @Ids";
        var contacts = await conn.QueryAsync<Contact>(sql, new { Ids = packages.Select(q => q.ContactId).ToList() });
        foreach (var package in packages)
        {
            package.Contact = contacts.FirstOrDefault(q => q.Id == package.ContactId);
        }

        sql = "select * from goods where PackageId in @PackageIds";
        var goods = await conn.QueryAsync<Good>(sql, new { PackageIds = packages.Select(q => q.Id).ToList() });
        foreach (var package in packages)
        {
            package.Goods = goods.Where(q => q.PackageId == package.Id);
        }

        return order;
    }

    public async Task<(int, IEnumerable<Order>)> List(OrderListRequestDto request)
    {
        var builder = new SqlBuilder();
        builder.Where("UserId = @UserId", new { UserId = request.UserId });
        if (request.Status != null)
        {
            builder.Where("Status = @Status", new { Status = request.Status.Value });
        }

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            builder.Where("Code = @Code", new { Code = request.Code });
        }

        if (!string.IsNullOrWhiteSpace(request.Reference))
        {
            builder.Where("Reference = @Reference", new { Reference = request.Reference });
        }

        if (request.StartTime.HasValue)
        {
            builder.Where("Departure >= @StartTime", new { StartTime = request.StartTime.Value });
        }

        if (request.EndTime.HasValue)
        {
            builder.Where("Departure < @EndTime", new { EndTime = request.EndTime.Value });
        }

        var countTemplate = builder.AddTemplate(@"select count(*) from orders /**where**/");
        var queryTemplate =
            builder.AddTemplate(
                $"select * from orders /**where**/ order by CreateTime desc limit {request.PageSize} offset {((request.PageIndex - 1) * request.PageSize)}");

        using var conn = await _dbContext.GetConnection();
        var count = await conn.ExecuteScalarAsync<int>(countTemplate.RawSql, countTemplate.Parameters);
        var orders = await conn.QueryAsync<Order>(queryTemplate.RawSql, queryTemplate.Parameters);

        var sql = "select * from packages where OrderCode in @OrderCodes";
        var packages = await conn.QueryAsync<Package>(sql, new { OrderCodes = orders.Select(q => q.Code).ToList() });
        foreach (var order in orders)
        {
            order.Packages = packages.Where(q => q.OrderCode == order.Code).ToList();
        }

        sql = "select * from contacts where Id in @Ids";
        var contacts = await conn.QueryAsync<Contact>(sql, new { Ids = packages.Select(q => q.ContactId).ToList() });
        foreach (var package in packages)
        {
            package.Contact = contacts.FirstOrDefault(q => q.Id == package.ContactId);
        }

        sql = "select * from goods where PackageId in @PackageIds";
        var goods = await conn.QueryAsync<Good>(sql, new { PackageIds = packages.Select(q => q.Id).ToList() });
        foreach (var package in packages)
        {
            package.Goods = goods.Where(q => q.PackageId == package.Id);
        }

        return (count, orders);
    }
}