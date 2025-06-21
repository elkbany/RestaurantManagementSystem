using Mapster;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Staff;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaffService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<StaffDto>> GetAllStaffAsync(CancellationToken cancellationToken = default)
        {
            var staff = await _unitOfWork.Repository<Staff>().GetAllAsync(cancellationToken);
            return staff.Adapt<IEnumerable<StaffDto>>();
        }

        public async Task<StaffDto> GetStaffByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var staff = await _unitOfWork.Repository<Staff>().GetByIdAsync(id, cancellationToken);
            return staff.Adapt<StaffDto>();
        }

        public async Task AddStaffAsync(StaffDto staffDto, CancellationToken cancellationToken = default)
        {
            var staff = staffDto.Adapt<Staff>();
            await _unitOfWork.Repository<Staff>().AddAsync(staff, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            staffDto.Id = staff.Id;
        }

        public async Task UpdateStaffAsync(StaffDto staffDto, CancellationToken cancellationToken = default)
        {
            var staff = staffDto.Adapt<Staff>();
            _unitOfWork.Repository<Staff>().Update(staff);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteStaffAsync(int id, CancellationToken cancellationToken = default)
        {
            var staff = await _unitOfWork.Repository<Staff>().GetByIdAsync(id, cancellationToken);
            if (staff != null)
            {
                _unitOfWork.Repository<Staff>().Delete(staff);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task AssignOrderAsync(int staffId, int orderId, CancellationToken cancellationToken = default)
        {
            var staff = await _unitOfWork.Repository<Staff>().GetByIdAsync(staffId, cancellationToken);
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId, cancellationToken);
            if (staff != null && order != null)
            {
                staff.AssignedOrders.Add(order);
                _unitOfWork.Repository<Staff>().Update(staff);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
