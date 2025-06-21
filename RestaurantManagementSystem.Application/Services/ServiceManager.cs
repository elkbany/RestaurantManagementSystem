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
        private readonly Lazy<ITableService> _tableService;
        private readonly Lazy<IStaffService> _staffService;


        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _categoryService = new Lazy<ICategoryService>(() => new CategoryService(unitOfWork));
            _menuItemService = new Lazy<IMenuItemService>(() => new MenuItemService(unitOfWork));
            _orderService = new Lazy<IOrderService>(() => new OrderService(unitOfWork, this));
            _tableService = new Lazy<ITableService>(() => new TableService(unitOfWork));
            _staffService = new Lazy<IStaffService>(() => new StaffService(unitOfWork));
        }

        public ICategoryService CategoryService => _categoryService.Value;
        public IMenuItemService MenuItemService => _menuItemService.Value;
        public IOrderService OrderService => _orderService.Value;
        public ITableService TableService => _tableService.Value;
        public IStaffService StaffService => _staffService.Value;
    }
}