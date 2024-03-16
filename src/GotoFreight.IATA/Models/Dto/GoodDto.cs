using System.ComponentModel.DataAnnotations;

namespace GotoFreight.IATA.Models.Dto;

public class GoodDto
{
    [Required(ErrorMessage = "Commodity is required")]
    public string Commodity { get; set; }

    // [Required(ErrorMessage = "Pcs is required")]
    public double Pcs { get; set; }
    
    // [Required(ErrorMessage = "Price is required")]
    public double Price { get; set; }
    
    // [Required(ErrorMessage = "Amount is required")]
    public double Amount { get; set; }

    // [Required(ErrorMessage = "HsCode is required")]
    public string HsCode { get; set; }
    
    // [Required(ErrorMessage = "Usage is required")]
    public string Usage { get; set; }
    
    [Required(ErrorMessage = "Materia is required")]
    public string Materia { get; set; }
    
    // [Required(ErrorMessage = "Orginal is required")]
    public string Orginal { get; set; }
    
    [Required(ErrorMessage = "Photo is required")]
    public string Photo { get; set; }
    
    public string AiResult { get; set; }
}