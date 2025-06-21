using Microsoft.AspNetCore.Builder;

namespace RestaurantManagementSystem.PresentationLayer.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseBusinessHours(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BusinessHoursMiddleware>();
        }
    }
}