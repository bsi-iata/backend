namespace GotoFreight.IATA.Models.Entity;

public class Contact
{
    public long Id { get; set; }

    public string Name { get; set; }
    
    public string Phone { get; set; }
    
    public string Email { get; set; }
    
    public string Contry { get; set; }
    
    public string State { get; set; }
    
    public string City { get; set; }
    
    public string Zip { get; set; }
    
    public string Address { get; set; }
    
    public DateTime CreateTime { get; set; }
    
    public DateTime UpdateTime { get; set; }
}