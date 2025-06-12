using Mapster;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Order;
using RestaurantManagementSystem.Application.DTOs.OrderItem;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Repositories;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            return orders.Adapt<IEnumerable<OrderDto>>();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            return order?.Adapt<OrderDto>();
        }

        public async Task AddOrderAsync(OrderDto orderDto)
        {
            if (orderDto.CustomerId <= 0 || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
                throw new ArgumentException("Invalid order data.");

            if (orderDto.Id != 0)
                throw new ArgumentException("Id should not be set for new order.");

            var order = orderDto.Adapt<Order>();
            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(int id, OrderDto orderDto)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order != null)
            {
                orderDto.Adapt(order);
                _unitOfWork.Repository<Order>().Update(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order != null)
            {
                _unitOfWork.Repository<Order>().Delete(order);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}