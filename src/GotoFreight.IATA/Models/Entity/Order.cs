namespace GotoFreight.IATA.Models.Entity;

public class Order
{
    public string Code { get; set; }

    public long UserId { get; set; }

    public string Airline { get; set; }

    public string Flight { get; set; }

    public int Status { get; set; }

    public string Reference { get; set; }
    
    public string Remark { get; set; }

    public string DepartureAddress { get; set; }
    
    public DateTime Departure { get; set; }
    
    public string ArrivalAddress { get; set; }
    
    public DateTime Arrival { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }


    //extensions
    public IEnumerable<Package> Packages { get; set; }
}