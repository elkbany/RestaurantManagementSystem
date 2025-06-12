using Mapster;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.MenuItem;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Repositories;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync()
        {
            var menuItems = await _unitOfWork.Repository<MenuItem>().GetAllAsync();
            return menuItems.Adapt<IEnumerable<MenuItemDto>>();
        }

        public async Task<MenuItemDto> GetMenuItemByIdAsync(int id)
        {
            var menuItem = await _unitOfWork.Repository<MenuItem>().GetByIdAsync(id);
            return menuItem?.Adapt<MenuItemDto>();
        }

        public async Task AddMenuItemAsync(MenuItemDto menuItemDto)
        {
            if (string.IsNullOrWhiteSpace(menuItemDto.Name) || menuItemDto.Price < 0 || menuItemDto.PreparationTime <= 0)
                throw new ArgumentException("Invalid menu item data.");

            if (menuItemDto.Id != 0)
                throw new ArgumentException("Id should not be set for new menu item.");

            var menuItem = menuItemDto.Adapt<MenuItem>();
            await _unitOfWork.Repository<MenuItem>().AddAsync(menuItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateMenuItemAsync(int id, MenuItemDto menuItemDto)
        {
            if (string.IsNullOrWhiteSpace(menuItemDto.Name) || menuItemDto.Price < 0 || menuItemDto.PreparationTime <= 0)
                throw new ArgumentException("Invalid menu item data.");

            var menuItem = await _unitOfWork.Repository<MenuItem>().GetByIdAsync(id);
            if (menuItem != null)
            {
                menuItemDto.Adapt(menuItem);
                _unitOfWork.Repository<MenuItem>().Update(menuItem);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteMenuItemAsync(int id)
        {
            var menuItem = await _unitOfWork.Repository<MenuItem>().GetByIdAsync(id);
            if (menuItem != null)
            {
                _unitOfWork.Repository<MenuItem>().Delete(menuItem);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}