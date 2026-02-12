using System.ComponentModel.DataAnnotations;

namespace ReLoop.Client.Models;

public class SellItemModel
{
    [Required(ErrorMessage = "Item name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be 2-100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be 10-500 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100,000")]
    public decimal Price { get; set; }
}
