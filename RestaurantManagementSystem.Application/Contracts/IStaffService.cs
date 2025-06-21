using RestaurantManagementSystem.Application.DTOs.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Contracts
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffDto>> GetAllStaffAsync(CancellationToken cancellationToken = default);
        Task<StaffDto> GetStaffByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddStaffAsync(StaffDto staffDto, CancellationToken cancellationToken = default);
        Task UpdateStaffAsync(StaffDto staffDto, CancellationToken cancellationToken = default);
        Task DeleteStaffAsync(int id, CancellationToken cancellationToken = default);
        Task AssignOrderAsync(int staffId, int orderId, CancellationToken cancellationToken = default);
    }
}
