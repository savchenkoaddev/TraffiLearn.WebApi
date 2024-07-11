using TraffiLearn.Application.DTO.Categories.Request;
using TraffiLearn.Application.DTO.Categories.Response;

namespace TraffiLearn.Application.ServiceContracts
{
    public interface ICategoryService
    {
        Task AddAsync(CategoryRequest? request);

        Task DeleteAsync(Guid? categoryId);

        Task UpdateAsync(Guid? categoryId, CategoryRequest? request);

        Task<IEnumerable<CategoryResponse>> GetAllAsync();

        Task<CategoryResponse> GetByIdAsync(Guid? categoryId);
    }
}
