using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Category;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.PresentationLayer.Controllers
{
    [Route("categories/[action]/{id:int?}")]
    public class CategoriesController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Route("~/categories")]
        public async Task<IActionResult> Index()
        {
            var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
            var activeCategories = categories.Where(c => c.IsActive && c.MenuItemCount > 0);
            return View(activeCategories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);
            if (category == null || category.MenuItemCount == 0) // إخفاء لو مفيش عناصر
                return NotFound();
            return View(category);
        }

        public IActionResult Create()
        {
            return View(new CategoryDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return View(categoryDto);
            await _serviceManager.CategoryService.AddCategoryAsync(categoryDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);
            if (category == null || (await _serviceManager.CategoryService.GetAllCategoriesAsync()).Count(c => c.Id == id && c.MenuItemCount == 0) > 0)
                return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryDto categoryDto)
        {
            if (id != categoryDto.Id || !ModelState.IsValid) return NotFound();
            await _serviceManager.CategoryService.UpdateCategoryAsync(id, categoryDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _serviceManager.CategoryService.GetCategoryByIdAsync(id);
            if (category == null || (await _serviceManager.CategoryService.GetAllCategoriesAsync()).Count(c => c.Id == id && c.MenuItemCount == 0) > 0)
                return NotFound();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceManager.CategoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}