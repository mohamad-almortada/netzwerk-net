using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;
using Netzwerk.Model;

namespace Netzwerk.Services;

public class CategoryService(ApiContext apiContext, IMapper mapper) : ICategoryService
{
    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
        var category = mapper.Map<Category>(categoryDto);
        await apiContext.Categories.AddAsync(category);
        await apiContext.SaveChangesAsync();
        return mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto?> GetCategoryAsync(int categoryId)
    {
        var category = await apiContext.Categories.FindAsync(categoryId);
        return category == null ? null : mapper.Map<CategoryDto>(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await apiContext.Categories.ToListAsync();
        return mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto)
    {
        var category = await apiContext.Categories.FindAsync(categoryId);
        if (category == null) return null;
        var map = mapper.Map<Category>(categoryDto);
        category.Name = map.Name;
        await apiContext.SaveChangesAsync();
        return mapper.Map<CategoryDto>(category);
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var category = await apiContext.Categories.FindAsync(categoryId);
        if (category == null) return false;
        var map = mapper.Map<Category>(category);
        apiContext.Categories.Remove(category);
        await apiContext.SaveChangesAsync();
        return true;
    }
}