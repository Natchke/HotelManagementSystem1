using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Repository.Abstraction
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, int pageNumber, int pageSize, string includeProperties = null);
        Task<List<T>> GetAllAsync(int pageNumber, int pageSize, string includeProperties = null);
        Task<T> GetAllAsync(Expression<Func<T, bool>> filter, string includeProperties = null);
        Task AddAsync(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
