using RestaurantManagementSystem.Application.DTOs.MenuItem;
using RestaurantManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Contracts
{
    public interface IMenuItemService
    {
        public Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync();
        public Task<MenuItemDto> GetMenuItemByIdAsync(int id);
        public Task AddMenuItemAsync(MenuItemDto menuItemDto);
        public Task UpdateMenuItemAsync(int id, MenuItemDto menuItemDto);
        public Task DeleteMenuItemAsync(int id);
        Task UpdateAvailabilityAsync();
        Task ResetDailyDataAsync();
        decimal? ApplyHappyHourDiscount(int menuItemId, decimal originalPrice);
    }
}
