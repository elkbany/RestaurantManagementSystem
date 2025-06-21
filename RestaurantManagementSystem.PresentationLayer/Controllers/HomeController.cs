using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.PresentationLayer.Models;
using System.Diagnostics;

namespace RestaurantManagementSystem.PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public HomeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IActionResult> Index()
        {
            // Await the tasks to get the IEnumerable results
            var tables = await _serviceManager.TableService.GetAllTablesAsync();
            var orders = await _serviceManager.OrderService.GetAllOrdersAsync();
            var staff = await _serviceManager.StaffService.GetAllStaffAsync();

            // Use CountAsync on the awaited results
            ViewBag.TableCount = await Task.FromResult(tables.Count());
            ViewBag.OrderCount = await Task.FromResult(orders.Count());
            ViewBag.StaffCount = await Task.FromResult(staff.Count());

            return View();
        }
    }
}
