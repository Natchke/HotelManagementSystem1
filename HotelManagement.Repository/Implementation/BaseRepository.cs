using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Repository.Abstraction;
using HotelManagement.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repository.Implementation
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbSet<T> _dbset;
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }



        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, string includeProperties = null)
        {
            IQueryable<T> query = _dbset;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query
                        .Include(includeProperty)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);

                }

                return await query.ToListAsync();
            }

            var entities = await query
                .Where(filter)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return entities;
        }

        public async Task<List<T>> GetAllAsync(int pageNumber, int pageSize, string includeProperties = null)
        {
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                IQueryable<T> query = _dbset;

                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                return await
                    query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }

            return await
                _dbset
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            IQueryable<T> query = _dbset;

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(filter);

        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();

            foreach (var entity in _context.ChangeTracker.Entries<T>().Where(e => e.State == EntityState.Modified))
            {
                _context.Entry(entity.Entity).Reload();
            }
        }

        public async Task AddAsync(T entity) => await _dbset.AddAsync(entity);
        public void Remove(T entity) => _dbset.Remove(entity);
        public void RemoveRange(IEnumerable<T> entities) => _dbset.RemoveRange(entities);
        public async Task Save() => await _context.SaveChangesAsync();
    }
}
