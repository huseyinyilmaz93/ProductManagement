using System.ComponentModel.DataAnnotations;

namespace PM.Api.Dtos;

public class UpdateProduct
{
    [Required]
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CategoryId { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }

}

