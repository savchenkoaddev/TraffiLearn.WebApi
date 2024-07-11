using TraffiLearn.Application.DTO.Categories.Request;
using TraffiLearn.Application.DTO.Categories.Response;

namespace TraffiLearn.Application.ServiceContracts
{
    public interface ICategoryService
    {
        Task AddAsync(CategoryRequest? item);

        Task DeleteAsync(Guid? key);

        Task UpdateAsync(Guid? key, CategoryRequest? item);

        Task<IEnumerable<CategoryResponse>> GetAllAsync();

        Task<CategoryResponse> GetByIdAsync(Guid? key);
    }
}
