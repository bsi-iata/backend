using System.ComponentModel.DataAnnotations;

namespace GotoFreight.IATA.Models.Dto;

public class ContactDto
{
    // [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    // [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; }
    
    // [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    
    // [Required(ErrorMessage = "Contry is required")]
    public string Contry { get; set; }
    
    // [Required(ErrorMessage = "State is required")]
    public string State { get; set; }
    
    // [Required(ErrorMessage = "City is required")]
    public string City { get; set; }
    
    // [Required(ErrorMessage = "Zip is required")]
    public string Zip { get; set; }
    
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
}
