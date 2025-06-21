using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Contracts
{
    public interface IServiceManager
    {
        public ICategoryService CategoryService { get; }
        public IMenuItemService MenuItemService { get; }
        public IOrderService OrderService { get; }
        public ITableService TableService { get; }
        public IStaffService StaffService { get; }
    }
}
