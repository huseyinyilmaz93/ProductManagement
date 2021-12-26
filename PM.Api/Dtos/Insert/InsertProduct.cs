using System.ComponentModel.DataAnnotations;

namespace PM.Api.Dtos;

public class InsertProduct
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    public string CategoryId { get; set; }
    [Required]
    public decimal? Price { get; set; }
    [Required]
    public string? Currency { get; set; }
}

