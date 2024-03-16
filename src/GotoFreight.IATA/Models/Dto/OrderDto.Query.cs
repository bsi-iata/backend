using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Models.Dto;

public class OrderListRequestDto
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }


    public long UserId { get; set; }

    public int? Status { get; set; }

    public string Code { get; set; }

    public string Reference { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}

public class OrderListResultDto
{
    public int Total { get; set; }

    public IEnumerable<OrderInfo> Items { get; set; }
}

public class OrderInfo
{
    public string NO { get; set; }

    public string Airline { get; set; }

    public string Flight { get; set; }

    public ContactDto Destination { get; set; }

    public PackageDto Package { get; set; }

    public string Goods { get; set; }

    public string Remark { get; set; }

    public int Status { get; set; }

    public string DepartureAddress { get; set; }
    
    public DateTime Departure { get; set; }
    
    public string ArrivalAddress { get; set; }
    
    public DateTime Arrival { get; set; }
    
    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}

public class OrderDetailDto : Order
{
}