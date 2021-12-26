using System.ComponentModel.DataAnnotations;

namespace PM.Api.Dtos
{
    public class UpdateCategory
    {
        [Required]
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

    }
}
