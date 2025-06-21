using Mapster;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.MenuItem;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
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
            var query = await _unitOfWork.Repository<MenuItem>().QueryAsync();
            var menuItems = await query.Include(m => m.Category).AsNoTracking().ToListAsync();
            return menuItems.Adapt<IEnumerable<MenuItemDto>>();
        }

        public async Task<MenuItemDto> GetMenuItemByIdAsync(int id)
        {
            var query = await _unitOfWork.Repository<MenuItem>().QueryAsync();
            var menuItem = await query.Include(m => m.Category).AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem != null)
            {
                var dto = menuItem.Adapt<MenuItemDto>();
                dto.DiscountedPrice = ApplyHappyHourDiscount(id, menuItem.Price);
                return dto;
            }
            return null;
        }

        public async Task AddMenuItemAsync(MenuItemDto menuItemDto)
        {
            if (string.IsNullOrWhiteSpace(menuItemDto.Name) || menuItemDto.Price < 0 || menuItemDto.PreparationTime < 0)
                throw new ArgumentException("Invalid menu item data.");
            if (!menuItemDto.IsAvailable)
                throw new ArgumentException("Cannot add unavailable item.");

            if (menuItemDto.Id != 0)
                throw new ArgumentException("Id should not be set for new menu item.");

            var menuItem = menuItemDto.Adapt<MenuItem>();
            await _unitOfWork.Repository<MenuItem>().AddAsync(menuItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateMenuItemAsync(int id, MenuItemDto menuItemDto)
        {
            if (string.IsNullOrWhiteSpace(menuItemDto.Name) || menuItemDto.Price < 0 || menuItemDto.PreparationTime < 0)
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

        public async Task UpdateAvailabilityAsync()
        {
            var query = await _unitOfWork.Repository<MenuItem>().QueryAsync();
            var menuItems = await query.ToListAsync();
            foreach (var item in menuItems)
            {
                if (item.DailyOrderCount > 50 && item.IsAvailable)
                    item.IsAvailable = false;
                else if (item.DailyOrderCount <= 50 && !item.IsAvailable)
                    item.IsAvailable = true;
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ResetDailyDataAsync()
        {
            var query = await _unitOfWork.Repository<MenuItem>().QueryAsync();
            var menuItems = await query.ToListAsync();
            foreach (var item in menuItems)
            {
                item.DailyOrderCount = 0;
                if (!item.IsAvailable && item.DailyOrderCount <= 50)
                    item.IsAvailable = true;
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public decimal? ApplyHappyHourDiscount(int menuItemId, decimal originalPrice)
        {
            var currentTime = DateTime.Now; 
            // Happy Hour: 6 PM - 8 PM
            if (currentTime.Hour >= 18 && currentTime.Hour < 20)
                return originalPrice * 0.8m; // 20% off
            return null; 
        }
    }
}