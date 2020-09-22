using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T> where T: class
    {

        T Get(int id);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null);
        void Insert(T item);
        void Remove(T item);
        void Remove(int id);


    }
}
