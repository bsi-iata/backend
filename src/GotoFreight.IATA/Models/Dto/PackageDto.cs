using System.ComponentModel.DataAnnotations;

namespace GotoFreight.IATA.Models.Dto;

public class PackageDto
{
    [Required(ErrorMessage = "Airline is required")]
    public string Airline { get; set; }

    [Required(ErrorMessage = "Flight is required")]
    public string Flight { get; set; }

    // [Required(ErrorMessage = "DepartureAddress is required")]
    public string DepartureAddress { get; set; }
    
    // [Required(ErrorMessage = "ArrivalAddress is required")]
    public string ArrivalAddress { get; set; }
    
    // [Required(ErrorMessage = "Departure is required")]
    public DateTime Departure { get; set; }
    
    // [Required(ErrorMessage = "Arrival is required")]
    public DateTime Arrival { get; set; }
    
    [Required(ErrorMessage = "Weight is required")]
    public double Weight { get; set; }

    [Required(ErrorMessage = "Volumn is required")]
    public double Volumn { get; set; }

    // [Required(ErrorMessage = "Quantity is required")]
    public double Quantity { get; set; }

    // [Required(ErrorMessage = "Remark is required")]
    public string Remark { get; set; }

    // [Required(ErrorMessage = "GoodsDesc is required")]
    // [MinLength(1, ErrorMessage = "GoodsDesc lenght is 10.")]
    public string GoodsDesc { get; set; }
}
