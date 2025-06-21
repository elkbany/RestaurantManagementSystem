using Microsoft.AspNetCore.Http;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.MenuItem;
using System;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.PresentationLayer.Middleware
{
    public class BusinessHoursMiddleware
    {
        private readonly RequestDelegate _next;

        public BusinessHoursMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var currentTime = DateTime.Now;
            var startTime = new TimeSpan(8, 0, 0); // 8 AM
            var endTime = new TimeSpan(22, 0, 0); // 10 PM
            var currentDayTime = currentTime.TimeOfDay;

            if (currentDayTime < startTime || currentDayTime > endTime)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Restaurant is closed. Operating hours: 8 AM - 10 PM.");
                return;
            }

            if (currentTime.Hour == 0 && currentTime.Minute == 0)
            {
                await ResetDailyData(context);
            }

            await _next(context);
        }

        private async Task ResetDailyData(HttpContext context)
        {
            var serviceManager = context.RequestServices.GetService<IServiceManager>();
            if (serviceManager != null)
            {
                var menuItems = await serviceManager.MenuItemService.GetAllMenuItemsAsync();
                foreach (var item in menuItems)
                {
                    var updatedItem = new MenuItemDto { Id = item.Id, DailyOrderCount = 0, IsAvailable = true };
                    await serviceManager.MenuItemService.UpdateMenuItemAsync(item.Id, updatedItem);
                }
            }
        }
    }
}