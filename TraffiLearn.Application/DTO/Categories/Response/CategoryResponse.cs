using System.ComponentModel.DataAnnotations;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.DTO.Categories.Response
{
    public sealed class CategoryResponse
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
