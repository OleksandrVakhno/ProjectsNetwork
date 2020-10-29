using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace ProjectsNetwork.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        private DbSet<T> _dbSet;


        public Repository(ApplicationDbContext db)
        {

            this._db = db;
            this._dbSet = this._db.Set<T>();

        }

        public T Get(params object[] ids)
        {
            return this._dbSet.Find(ids);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {

            IQueryable<T> query = this._dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }

            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public EntityEntry<T> Insert(T item)
        {
            return this._dbSet.Add(item);
        }

        public EntityEntry<T> Remove(T item)
        {
            return this._dbSet.Remove(item);
        }

        public EntityEntry<T> Remove(params object[] ids)
        {
            T entity = this._dbSet.Find(ids);
            return this._dbSet.Remove(entity);
        }

        public int Save()
        {
            return this._db.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return this._db.SaveChangesAsync();
        }
    }
}
