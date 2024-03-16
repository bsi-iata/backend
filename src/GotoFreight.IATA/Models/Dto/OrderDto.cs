using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Models.Dto;

public class OrderSubmitDto
{
    public string Code { get; set; }

    [JsonIgnore] public Order Order { get; set; }

    [Required(ErrorMessage = "Package is required")]
    public PackageDto Package { get; set; }

    // [Required(ErrorMessage = "Contact is required")]
    public ContactDto Contact { get; set; }

    [Required(ErrorMessage = "Goods is required")]
    [MinLength(1, ErrorMessage = "Goods count is 1.")]
    public IEnumerable<GoodDto> Goods { get; set; }
}