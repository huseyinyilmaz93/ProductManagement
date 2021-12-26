using System.ComponentModel.DataAnnotations;

namespace PM.Api.Dtos;

public class InsertCategory
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }

}

