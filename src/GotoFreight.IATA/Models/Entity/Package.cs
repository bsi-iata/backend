namespace GotoFreight.IATA.Models.Entity;

public class Package
{
    public long Id { get; set; }

    public string OrderCode { get; set; }

    public long ContactId { get; set; }

    public double Weight { get; set; }

    public double Volumn { get; set; }

    public double Quantity { get; set; }

    public string Remark { get; set; }

    public string GoodsDesc { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }


    //extensions
    public Order Order { get; set; }

    public Contact Contact { get; set; }

    public IEnumerable<Good> Goods { get; set; }
}