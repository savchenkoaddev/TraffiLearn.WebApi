using Riok.Mapperly.Abstractions;
using TraffiLearn.Application.DTO.Categories.Request;
using TraffiLearn.Application.DTO.Categories.Response;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Services.Mappers
{
    [Mapper]
    public partial class CategoryMapper
    {
        public partial DrivingCategory ToEntity(CategoryRequest request);

        public partial CategoryResponse ToResponse(DrivingCategory question);

        public partial IEnumerable<CategoryResponse> ToResponse(IEnumerable<DrivingCategory> questions);
    }
}
