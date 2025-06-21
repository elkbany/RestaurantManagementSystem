using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Table;

namespace RestaurantManagementSystem.Presentation.Controllers
{
    [Route("tables/[action]/{id:int?}")]
    public class TablesController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public TablesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Route("~/tables")]
        public async Task<IActionResult> Index()
        {
            var tables = await _serviceManager.TableService.GetAllTablesAsync();
            return View(tables);
        }

        public async Task<IActionResult> Details(int id)
        {
            var table = await _serviceManager.TableService.GetTableByIdAsync(id);
            return View(table);
        }

        public IActionResult Create()
        {
            return View(new TableDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TableDto tableDto)
        {
            if (!ModelState.IsValid)
            {
                return View(tableDto);
            }

            await _serviceManager.TableService.AddTableAsync(tableDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TableDto tableDto)
        {
            if (!ModelState.IsValid)
            {
                return View(tableDto);
            }

            await _serviceManager.TableService.UpdateTableAsync(tableDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default) 
        {
            await _serviceManager.TableService.DeleteTableAsync(id, cancellationToken); 
            return RedirectToAction(nameof(Index));
        }
    }
}
