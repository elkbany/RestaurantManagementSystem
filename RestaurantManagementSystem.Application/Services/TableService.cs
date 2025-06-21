using Mapster;
using RestaurantManagementSystem.Application.Contracts;
using RestaurantManagementSystem.Application.DTOs.Reservation;
using RestaurantManagementSystem.Application.DTOs.Table;
using RestaurantManagementSystem.Domain.Entities;
using RestaurantManagementSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Application.Services
{
    public class TableService : ITableService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TableDto>> GetAllTablesAsync(CancellationToken cancellationToken = default)
        {
            var tables = await _unitOfWork.Repository<Table>().GetAllAsync(cancellationToken);
            return tables.Adapt<IEnumerable<TableDto>>();
        }

        public async Task<TableDto> GetTableByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var table = await _unitOfWork.Repository<Table>().GetByIdAsync(id, cancellationToken);
            return table.Adapt<TableDto>();
        }

        public async Task AddTableAsync(TableDto tableDto, CancellationToken cancellationToken = default)
        {
            var table = tableDto.Adapt<Table>();
            await _unitOfWork.Repository<Table>().AddAsync(table, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            tableDto.Id = table.Id;
        }

        public async Task UpdateTableAsync(TableDto tableDto, CancellationToken cancellationToken = default)
        {
            var table = tableDto.Adapt<Table>();
            _unitOfWork.Repository<Table>().Update(table);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTableAsync(int id, CancellationToken cancellationToken = default)
        {
            var table = await _unitOfWork.Repository<Table>().GetByIdAsync(id, cancellationToken);
            if (table != null)
            {
                _unitOfWork.Repository<Table>().Delete(table);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task AddReservationAsync(ReservationDto reservationDto, CancellationToken cancellationToken = default)
        {
            var reservation = reservationDto.Adapt<Reservation>();
            var table = await _unitOfWork.Repository<Table>().GetByIdAsync(reservationDto.TableId, cancellationToken);
            if (table != null && !table.IsReserved)
            {
                table.IsReserved = true;
                table.ReservationTime = reservation.ReservationDateTime;
                table.Reservations.Add(reservation);
                _unitOfWork.Repository<Table>().Update(table);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                reservationDto.Id = reservation.Id;
            }
            else
            {
                throw new Exception("Table is already reserved or does not exist.");
            }
        }
    }
}
