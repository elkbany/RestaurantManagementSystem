using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Domain.Repositories;
using System;

namespace RestaurantManagementSystem.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IMenuItemService> _menuItemService;
        private readonly Lazy<IOrderService> _orderService;

        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _categoryService = new Lazy<ICategoryService>(() => new CategoryService(unitOfWork));
            _menuItemService = new Lazy<IMenuItemService>(() => new MenuItemService(unitOfWork));
            _orderService = new Lazy<IOrderService>(() => new OrderService(unitOfWork));
        }

        public ICategoryService CategoryService => _categoryService.Value;
        public IMenuItemService MenuItemService => _menuItemService.Value;
        public IOrderService OrderService => _orderService.Value;
    }
}