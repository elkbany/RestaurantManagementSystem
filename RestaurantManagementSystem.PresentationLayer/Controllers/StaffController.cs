using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Staff;

namespace RestaurantManagementSystem.Presentation.Controllers
{
    [Route("staff/[action]/{id:int?}")]
    public class StaffController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public StaffController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Route("~/staff")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var staff = await _serviceManager.StaffService.GetAllStaffAsync(cancellationToken);
            return View(staff);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
        {
            var staff = await _serviceManager.StaffService.GetStaffByIdAsync(id, cancellationToken);
            if (staff == null) return NotFound();
            return View(staff);
        }

        public IActionResult Create()
        {
            return View(new StaffDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(StaffDto staffDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return View(staffDto);
            await _serviceManager.StaffService.AddStaffAsync(staffDto, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StaffDto staffDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return View(staffDto);
            await _serviceManager.StaffService.UpdateStaffAsync(staffDto, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            await _serviceManager.StaffService.DeleteStaffAsync(id, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AssignOrder(int staffId, int orderId, CancellationToken cancellationToken = default)
        {
            await _serviceManager.StaffService.AssignOrderAsync(staffId, orderId, cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}
