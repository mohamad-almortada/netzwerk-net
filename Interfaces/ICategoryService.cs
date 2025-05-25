using Netzwerk.DTOs;

namespace Netzwerk.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
    Task<CategoryDto?> GetCategoryAsync(int categoryId);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto?> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(int categoryId);
}

