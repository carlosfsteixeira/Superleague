using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using Superleague.Data.Entities;
using System.Threading.Tasks;
using System.Security.Policy;

namespace Superleague.Data
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public T GetById(int id)
        {
            return  _context.Set<T>().FirstOrDefault(e => e.Id == id);
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            await SaveAllAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);

            await SaveAllAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);

            await SaveAllAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {

            var result = await _context.SaveChangesAsync() > 0;
            return result;
        }

        public bool SaveAll()
        {

            var result = _context.SaveChanges() > 0;
            return result;
        }


        public void RemoveRange(T entity)
        {
            _context.Set<T>().RemoveRange(entity);

            SaveAll();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);

            await SaveAllAsync();
        }
    }
}
