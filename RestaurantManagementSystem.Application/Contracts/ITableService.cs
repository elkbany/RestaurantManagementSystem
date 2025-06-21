using RestaurantManagementSystem.Application.DTOs.Reservation;
using RestaurantManagementSystem.Application.DTOs.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Contracts
{
    public interface ITableService
    {
        Task<IEnumerable<TableDto>> GetAllTablesAsync(CancellationToken cancellationToken = default);
        Task<TableDto> GetTableByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddTableAsync(TableDto tableDto, CancellationToken cancellationToken = default);
        Task UpdateTableAsync(TableDto tableDto, CancellationToken cancellationToken = default);
        Task DeleteTableAsync(int id, CancellationToken cancellationToken = default);
        Task AddReservationAsync(ReservationDto reservationDto, CancellationToken cancellationToken = default);
    }
}
