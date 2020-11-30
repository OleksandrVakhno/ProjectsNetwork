using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectsNetwork.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Runtime.Serialization;

namespace ProjectsNetwork.DataAccess.Repositories.MockRepositories
{
    public class MockRepository<T> where T : class
    {
        

        protected readonly Dictionary<string, T> db = new Dictionary<string, T>();
        protected bool failure = false;

        public T Get(params object[] ids)
        {
            StringBuilder key = new StringBuilder();
            foreach (var id in ids) {
                key.Append(id.ToString());
            }
            var k = key.ToString();
            return db.GetValueOrDefault(k);
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = this.db.ToList().Select(o => o.Value).AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }


        public EntityEntry<T> Remove(T item)
        {

            if (failure)
            {
                return null;
            }
            
            foreach( var obj in db)
            {
                if(obj.Value.Equals(item))
                {
                    T val = obj.Value;
                    if (db.Remove(obj.Key)) {
                        return (EntityEntry<T>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<T>));
                    }
                    else
                    {
                        return null;
                    }
                    
                }
            }

            return null;
        }

        public EntityEntry<T> Remove(params object[] ids)
        {
            if (failure)
            {
                return null;
            }

            T val = db[ids.ToString()];
            if (db.Remove(ids.ToString()))
            {
                return (EntityEntry<T>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<T>));
            }
            else
            {
                return null;
            }
        }

        public int Save()
        {
            if (failure)
            {
                return 0;
            }
            return 1;
        }

        public Task<int> SaveAsync()
        {
            if (failure)
            {
                return new Task<int>(() => 0);
            }
            return new Task<int>(() => 1);
        }

        public void setFailure(bool val)
        {
            this.failure = val;
        }

    }
}
