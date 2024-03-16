namespace GotoFreight.IATA.Models.Entity;

public class Good
{
    public long Id { get; set; }
    
    public long PackageId { get; set; }

    public string Commodity { get; set; }

    public double Pcs { get; set; }
    
    public double Price { get; set; }
    
    public double Amount { get; set; }

    public string HsCode { get; set; }
    
    public string Usage { get; set; }
    
    public string Materia { get; set; }
    
    public string Orginal { get; set; }
    
    public string Photo { get; set; }
    
    public string AiResult { get; set; }
    
    public DateTime CreateTime { get; set; }
    
    public DateTime UpdateTime { get; set; }
    
    
    //extensions
    public Package Package { get; set; }
}