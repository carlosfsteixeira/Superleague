using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Superleague.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);
    }
}