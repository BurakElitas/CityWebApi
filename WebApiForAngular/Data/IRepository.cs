using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApiForAngular.Data
{
   public interface IRepository
    {
        int Insert<T>(T obj) where T:class;
        Task<EntityEntry<T>> InsertAsync<T>(T obj) where T : class;
        int Update<T>(T obj) where T : class;
        int Delete<T>(T obj) where T : class;
        T Find<T>(Expression<Func<T, bool>> where) where T : class;
        Task<T> FindAsync<T>(Expression<Func<T, bool>> where) where T : class;
        List<T> List<T>() where T : class;
        List<T> List<T>(Expression<Func<T, bool>> where) where T : class;
        IQueryable<T> ListQueryable<T>() where T : class;
        int Save();
        Task<int> SaveAsync();



    }
}
