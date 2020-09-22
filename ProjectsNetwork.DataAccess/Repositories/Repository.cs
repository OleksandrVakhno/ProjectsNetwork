using Microsoft.EntityFrameworkCore;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T:class
    {

        private readonly ApplicationDbContext _db;
        private DbSet<T> _dbSet;


        public Repository(ApplicationDbContext db)
        {

            this._db = db;
            this._dbSet = this._db.Set<T>();

        }

        public T Get(int id)
        {
            return this._dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = this._dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }

        public void Insert(T item)
        {
            this._dbSet.Add(item);
        }

        public void Remove(T item)
        {
            this._dbSet.Remove(item);
        }

        public void Remove(int id)
        {
            T entity = this._dbSet.Find(id);
            this._dbSet.Remove(entity);
        }
    }
}
