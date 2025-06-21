using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Application.Contracts;

namespace RestaurantManagementSystem.Presentation.Controllers
{
    [Route("analytics/[action]")]
    public class AnalyticsController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public AnalyticsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Route("~/analytics")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var salesData = await _serviceManager.OrderService.GetSalesAnalyticsAsync(cancellationToken);
            ViewBag.SalesData = salesData;
            return View();
        }
    }
}
