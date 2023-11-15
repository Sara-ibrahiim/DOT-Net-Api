using Core.Entities;
using Infrastructure.Sepecifcations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepository <T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int? id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetEntityWithSpecificationsAsync(ISepecifcations<T> specs);
        Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISepecifcations<T> specs);
       Task <int> CountAsync(ISepecifcations<T> sepecifcations);
        Task Add (T entity);
        void Update (T entity);
        void Delete (T entity);
    }
}
