using WealthTrack.Application.DTO;
using WealthTrack.Domain.Entities;
using WealthTrack.Domain.Exceptions;
using WealthTrack.Infrastructure.Repository;

namespace WealthTrack.Application.Services.CategoryS;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<User> _userRepository;

    public CategoryService(IRepository<Category> categoryRepository, 
        IRepository<User> userRepository)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(int userId)
    {
        var categories = await _categoryRepository
            .GetAllByFilterAsync(
                c => c.User!.Id == userId,
                c => c.User!);

        return categories
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => c.AsDto())
            .ToList();
    }

    public async Task CreateCategoryAsync(CreateCategoryDto dto, int userId)
    {
        var user = await _userRepository.GetOneAsync(u => u.Id == userId) ??
                   throw new UserNotFoundException();
        var category = dto.AsEntity(user);
        
        _userRepository.Attach(user);
        
        await _categoryRepository.CreateAsync(category);
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(int id, UpdateCategoryDto dto, int userId)
    {
        var category = await _categoryRepository.GetOneAsync(
            c => c.Id == id,
            c => c.User!
        );

        if (category is null) throw new CategoryNotFoundException();
        if (category.User!.Id != userId) throw new AccessDeniedException();
        
        dto.UpdateEntity(category);

        _userRepository.Attach(category.User);
        
        await _categoryRepository.UpdateAsync(id, category);
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id, int userId)
    {
        var category = await _categoryRepository.GetOneAsync(
            c => c.Id == id,
            c => c.User!
        );

        if (category is null) throw new CategoryNotFoundException();
        if (category.User!.Id != userId) throw new AccessDeniedException();

        await _categoryRepository.SoftDeleteAsync(id);
        await _categoryRepository.SaveChangesAsync();
    }
}