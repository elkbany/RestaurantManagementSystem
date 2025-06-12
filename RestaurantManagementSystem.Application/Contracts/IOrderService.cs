using RestaurantManagementSystem.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Contracts
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        public Task<OrderDto> GetOrderByIdAsync(int id);
        public Task AddOrderAsync(OrderDto orderDto);
        public Task UpdateOrderAsync(int id, OrderDto orderDto);
        public Task DeleteOrderAsync(int id);
    }
}
