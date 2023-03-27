using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IAlarmRepository
    {
        Task<IEnumerable<Alarm>> GetAllAsync();

        Task<Alarm> GetByIdAsync(Int32 id);

        Task AddAsync(Alarm alarm);

        Task<Alarm> UpdateAsync(Alarm alarm);

        Task DeleteAsync(Int32 id);
    }
}