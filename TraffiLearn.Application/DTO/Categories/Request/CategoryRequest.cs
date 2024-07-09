using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Application.DTO.Categories.Request
{
    public sealed class CategoryRequest
    {
        [StringLength(20)]
        [Required]
        public string? Code { get; set; }

        [StringLength(100)]
        [Required]
        public string? Description { get; set; }
    }
}
