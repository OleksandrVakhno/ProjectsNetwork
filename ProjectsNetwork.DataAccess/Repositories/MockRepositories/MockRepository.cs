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
        protected bool saveFailure = false;
        protected bool removeFailure = false;
        protected bool insertFailure = false;
        protected bool throwsException = false;
        protected bool updateFailure = false;
        protected readonly Exception e = new Exception("Test exception");

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

            if (removeFailure)
            {
                if (throwsException)
                {
                    throw this.e;
                }

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
            if (removeFailure)
            {
                if (throwsException)
                {
                    throw this.e;
                }

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
            if (saveFailure)
            {
                if (throwsException)
                {
                    throw this.e;
                }

                return 0;
            }
            return 1;
        }

        public Task<int> SaveAsync()
        {
            if (saveFailure)
            {
                if (throwsException)
                {
                    throw this.e;
                }

                return new Task<int>(() => 0);
            }
            return new Task<int>(() => 1);
        }

        public void setSaveFailure(bool val)
        {
            this.saveFailure = val;
        }

        public void setRemoveFailure(bool val)
        {
            this.removeFailure = val;
        }

        public void setInsertFailure(bool val)
        {
            this.insertFailure = val;
        }

        public void setUpdateFailure(bool val)
        {
            this.updateFailure = val;
        }

        public void setThrowsException(bool val)
        {
            this.throwsException = val;
        }
    }
}
