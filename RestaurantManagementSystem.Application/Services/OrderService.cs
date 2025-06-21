using Mapster;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Order;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Enums;
using RestaurantManagementSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceManager _serviceManager;

        public OrderService(IUnitOfWork unitOfWork, IServiceManager serviceManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var query = await _unitOfWork.Repository<Order>().QueryAsync();
            var orders = await query.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem).AsNoTracking().ToListAsync();
            return orders.Adapt<IEnumerable<OrderDto>>();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var query = await _unitOfWork.Repository<Order>().QueryAsync();
            var order = await query.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem).AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
            return order?.Adapt<OrderDto>();
        }

        public async Task AddOrderAsync(OrderDto orderDto)
        {
            if (orderDto == null) throw new ArgumentNullException(nameof(orderDto));
            if (orderDto.OrderItems == null || !orderDto.OrderItems.Any())
                throw new ArgumentException("Order must contain at least one item.");

            if (orderDto.OrderType == OrderType.Delivery && string.IsNullOrWhiteSpace(orderDto.DeliveryAddress))
                throw new ArgumentException("Delivery address is required for delivery orders.");

            decimal subTotal = 0m;
            var prepTimes = new List<int>();
            foreach (var item in orderDto.OrderItems)
            {
                var menuItem = await _serviceManager.MenuItemService.GetMenuItemByIdAsync(item.MenuItemId);
                if (menuItem == null || !menuItem.IsAvailable)
                    throw new ArgumentException($"Menu item with ID {item.MenuItemId} is unavailable.");

                decimal itemPrice = menuItem.DiscountedPrice ?? menuItem.Price;
                item.Subtotal = itemPrice * item.Quantity;
                subTotal += item.Subtotal;
                prepTimes.Add(menuItem.PreparationTime);
            }

            decimal tax = subTotal * 0.085m;
            decimal discount = CalculateDiscount(subTotal);
            orderDto.Total = subTotal + tax - discount;

            orderDto.EstimatedDeliveryTime = DateTime.Now.AddMinutes(prepTimes.Max() + 30);

            var order = orderDto.Adapt<Order>();
            await _unitOfWork.Repository<Order>().AddAsync(order);

            foreach (var item in orderDto.OrderItems)
            {
                var menuItem = await _unitOfWork.Repository<MenuItem>().GetByIdAsync(item.MenuItemId);
                if (menuItem != null)
                {
                    menuItem.DailyOrderCount += item.Quantity;
                    _unitOfWork.Repository<MenuItem>().Update(menuItem);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(int id, OrderDto orderDto)
        {
            if (orderDto == null) throw new ArgumentNullException(nameof(orderDto));
            if (id != orderDto.Id) throw new ArgumentException("Order ID mismatch.");

            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null) throw new ArgumentException("Order not found.");

            if (orderDto.OrderType == OrderType.Delivery && string.IsNullOrWhiteSpace(orderDto.DeliveryAddress))
                throw new ArgumentException("Delivery address is required for delivery orders.");

            decimal subTotal = 0m;
            var prepTimes = new List<int>();
            foreach (var item in orderDto.OrderItems)
            {
                var menuItem = await _serviceManager.MenuItemService.GetMenuItemByIdAsync(item.MenuItemId);
                if (menuItem == null || !menuItem.IsAvailable)
                    throw new ArgumentException($"Menu item with ID {item.MenuItemId} is unavailable.");

                decimal itemPrice = menuItem.DiscountedPrice ?? menuItem.Price;
                item.Subtotal = itemPrice * item.Quantity;
                subTotal += item.Subtotal;
                prepTimes.Add(menuItem.PreparationTime);
            }

            decimal tax = subTotal * 0.085m;
            decimal discount = CalculateDiscount(subTotal);
            orderDto.Total = subTotal + tax - discount;

            orderDto.EstimatedDeliveryTime = DateTime.Now.AddMinutes(prepTimes.Max() + 30);

            orderDto.Adapt(order);
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<(bool Success, string Message, OrderDto Order)> DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null) return (false, "Order not found.", null);

            if (order.Status == OrderStatus.Ready || order.Status == OrderStatus.Delivered)
                return (false, "Cannot delete Ready or Delivered orders.", order.Adapt<OrderDto>());

            _unitOfWork.Repository<Order>().Delete(order);
            await _unitOfWork.SaveChangesAsync();
            return (true, "Order deleted successfully.", order.Adapt<OrderDto>());
        }

        public async Task<Dictionary<string, decimal>> GetSalesAnalyticsAsync(CancellationToken cancellationToken = default)
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync(cancellationToken);
            var sales = new Dictionary<string, decimal>();
            foreach (var order in orders)
            {
                var date = order.OrderDate.ToString("yyyy-MM-dd");
                sales[date] = sales.GetValueOrDefault(date, 0) + order.Total;
            }
            return sales;
        }

        public async Task<Dictionary<string, object>> GetAnalyticsAsync(CancellationToken cancellationToken = default)
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync(cancellationToken);
            var sales = new Dictionary<string, decimal>();
            var hourCounts = new Dictionary<string, int>();

            foreach (var order in orders)
            {
                var date = order.OrderDate.ToString("yyyy-MM-dd");
                sales[date] = sales.GetValueOrDefault(date, 0) + order.Total;

                var hour = order.OrderDate.ToString("hh tt");
                hourCounts[hour] = hourCounts.GetValueOrDefault(hour, 0) + 1;
            }

            return new Dictionary<string, object>
            {
                { "Sales", sales },
                { "HourCounts", hourCounts }
            };
        }

        public async Task UpdateOrderStatusesAsync()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            foreach (var order in orders)
            {
                if (order.Status == OrderStatus.Pending && (DateTime.Now - order.OrderDate).TotalMinutes >= 5)
                    order.Status = OrderStatus.Preparing;
                else if (order.Status == OrderStatus.Preparing)
                {
                    var prepTime = order.OrderItems.Max(oi => _serviceManager.MenuItemService.GetMenuItemByIdAsync(oi.MenuItemId).Result.PreparationTime);
                    if ((DateTime.Now - order.OrderDate).TotalMinutes >= prepTime)
                        order.Status = OrderStatus.Ready;
                }
                _unitOfWork.Repository<Order>().Update(order);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        private decimal CalculateDiscount(decimal subTotal)
        {
            decimal discount = 0m;
            DateTime now = DateTime.Now;
            if (now.Hour >= 15 && now.Hour < 17) // 3-5 PM
                discount += subTotal * 0.20m; // 20% Happy Hour
            if (subTotal > 100m)
                discount += subTotal * 0.10m; // 10% Bulk
            return discount > subTotal ? subTotal : discount;
        }
    }
}