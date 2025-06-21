using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.MenuItem;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.PresentationLayer.Controllers
{
    [Route("menuitems/[action]/{id:int?}")]
    public class MenuItemsController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public MenuItemsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Route("~/menuitems")]
        public async Task<IActionResult> Index()
        {
            var menuItems = await _serviceManager.MenuItemService.GetAllMenuItemsAsync();
            await _serviceManager.MenuItemService.UpdateAvailabilityAsync(); // Update availability

            var viewModel = menuItems
                .Where(m => m.IsAvailable)
                .Select(m => new MenuItemDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    IsAvailable = m.IsAvailable,
                    PreparationTime = m.PreparationTime,
                    CategoryId = m.CategoryId,
                    CategoryName = m.CategoryName,
                    ImageUrl = m.ImageUrl,
                    DailyOrderCount = m.DailyOrderCount,
                    DiscountedPrice = _serviceManager.MenuItemService.ApplyHappyHourDiscount(m.Id, m.Price) != m.Price
                        ? _serviceManager.MenuItemService.ApplyHappyHourDiscount(m.Id, m.Price)
                        : (decimal?)null
                })
                .ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var menuItem = await _serviceManager.MenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null || !menuItem.IsAvailable)
                return NotFound();
            menuItem.DiscountedPrice = _serviceManager.MenuItemService.ApplyHappyHourDiscount(id, menuItem.Price) != menuItem.Price
                ? _serviceManager.MenuItemService.ApplyHappyHourDiscount(id, menuItem.Price)
                : (decimal?)null;
            return View(menuItem);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Name", "Name");
            return View(new MenuItemDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(MenuItemDto menuItemDto)
        {
            try
            {
                var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
                var category = categories.FirstOrDefault(c => c.Name == menuItemDto.CategoryName);
                if (category == null)
                {
                    ViewBag.Categories = new SelectList(categories, "Name", "Name");
                    ViewBag.ErrorMessage = "Invalid Category Name.";
                    return View(menuItemDto);
                }
                menuItemDto.CategoryId = category.Id;

                await _serviceManager.MenuItemService.AddMenuItemAsync(menuItemDto);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Name", "Name");
                return View(menuItemDto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await _serviceManager.MenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
                return NotFound();
            var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Name", "Name");
            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MenuItemDto menuItemDto)
        {
            try
            {
                if (id != menuItemDto.Id)
                    return NotFound();
                var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
                var category = categories.FirstOrDefault(c => c.Name == menuItemDto.CategoryName);
                if (category == null)
                {
                    ViewBag.ErrorMessage = "Invalid Category Name.";
                    ViewBag.Categories = new SelectList(categories, "Name", "Name");
                    return View(menuItemDto);
                }
                menuItemDto.CategoryId = category.Id;

                await _serviceManager.MenuItemService.UpdateMenuItemAsync(id, menuItemDto);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var categories = await _serviceManager.CategoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Name", "Name");
                return View(menuItemDto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _serviceManager.MenuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
                return NotFound();
            return View(menuItem);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceManager.MenuItemService.DeleteMenuItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}