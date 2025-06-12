using Mapster;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Category;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Repositories;
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
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            return categories.Adapt<IEnumerable<CategoryDto>>();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            return category?.Adapt<CategoryDto>();
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new ArgumentException("Category name is required.");

            if (categoryDto.Id != 0)
                throw new ArgumentException("Id should not be set for new category.");

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