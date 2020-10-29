using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsNetwork.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T> where T: class
    {

        T Get(params object[] ids);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);
        EntityEntry<T> Insert(T item);
        EntityEntry<T> Remove(T item);
        EntityEntry<T> Remove(params object[] ids);
        int Save();
        Task<int> SaveAsync();


    }
}
