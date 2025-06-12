using RestaurantManagementSystem.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Contracts
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        public Task<CategoryDto> GetCategoryByIdAsync(int id);
        public Task AddCategoryAsync(CategoryDto categoryDto);
        public Task UpdateCategoryAsync(int id, CategoryDto categoryDto);
        public Task DeleteCategoryAsync(int id);
    }
}
