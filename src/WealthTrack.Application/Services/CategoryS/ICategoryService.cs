using WealthTrack.Application.DTO;

namespace WealthTrack.Application.Services.CategoryS;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(int userId);
    Task CreateCategoryAsync(CreateCategoryDto dto, int userId);
    Task UpdateCategoryAsync(int id, UpdateCategoryDto dto, int userId);
    Task DeleteCategoryAsync(int id, int userId);
}