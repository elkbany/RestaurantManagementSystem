using Mapster;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Category;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var query = await _unitOfWork.Repository<Category>().QueryAsync();
            var categories = await query.Include(c => c.MenuItems).ToListAsync(); 
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                MenuItemCount = c.MenuItems?.Count ?? 0
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var query = await _unitOfWork.Repository<Category>().QueryAsync(); 
            var category = await query.Include(c => c.MenuItems).FirstOrDefaultAsync(c => c.Id == id);
            return category == null ? null : new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                MenuItemCount = category.MenuItems?.Count ?? 0
            };
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new ArgumentException("Category name is required.");

            var category = categoryDto.Adapt<Category>();
            await _unitOfWork.Repository<Category>().AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new ArgumentException("Category name is required.");

            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category != null)
            {
                categoryDto.Adapt(category);
                _unitOfWork.Repository<Category>().Update(category);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category != null)
            {
                _unitOfWork.Repository<Category>().Delete(category);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}