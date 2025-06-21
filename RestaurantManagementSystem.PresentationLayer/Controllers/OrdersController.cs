using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Order;
using RestaurantManagementSystem.Application.DTOs.OrderItem;
using RestaurantManagementSystem.Domain.Enums;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.PresentationLayer.Controllers
{
    [Route("orders/[action]/{id:int?}")]
    public class OrdersController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public OrdersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Route("~/orders")]
        public async Task<IActionResult> Index()
        {
            var orders = await _serviceManager.OrderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            ViewBag.Message = order.Status switch
            {
                OrderStatus.Preparing => "Your order is being prepared.",
                OrderStatus.Ready => "Your order is ready for pickup/delivery.",
                _ => "Your order is in progress."
            };
            return View(order);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.MenuItems = (await _serviceManager.MenuItemService.GetAllMenuItemsAsync()).Where(m => m.IsAvailable).ToList();
            return View(new OrderDto { OrderItems = new List<OrderItemDto>() });
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                orderDto.OrderItems ??= new List<OrderItemDto>();
                ViewBag.MenuItems = (await _serviceManager.MenuItemService.GetAllMenuItemsAsync()).Where(m => m.IsAvailable).ToList();
                return View(orderDto);
            }

            if (orderDto.OrderDate == default)
                orderDto.OrderDate = DateTime.Now;

            await _serviceManager.OrderService.AddOrderAsync(orderDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            ViewBag.MenuItems = (await _serviceManager.MenuItemService.GetAllMenuItemsAsync()).Where(m => m.IsAvailable).ToList();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                orderDto.OrderItems ??= new List<OrderItemDto>();
                ViewBag.MenuItems = (await _serviceManager.MenuItemService.GetAllMenuItemsAsync()).Where(m => m.IsAvailable).ToList();
                return View(orderDto);
            }

            if (id != orderDto.Id) return BadRequest();
            await _serviceManager.OrderService.UpdateOrderAsync(id, orderDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceManager.OrderService.DeleteOrderAsync(id);
            if (result.Order != null)
            {
                ViewBag.Message = result.Message;
                return View(result.Order);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _serviceManager.OrderService.DeleteOrderAsync(id);
            if (!result.Success)
            {
                ViewBag.Message = result.Message;
                return View(result.Order);
            }
            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}