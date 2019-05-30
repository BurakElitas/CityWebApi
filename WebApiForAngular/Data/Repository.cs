using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiForAngular.Models;

namespace WebApiForAngular.Data
{
    public class Repository : IRepository
    {
        DataContext _context;
        
        public Repository(DataContext context)
        {
            _context = context;
        }

        private DbSet<TT> GetType<TT>()where TT:class
        {
            return _context.Set<TT>();
        }

        public int Delete<T>(T obj) where T : class
        {
            GetType<T>().Remove(obj);
            return Save();
            
        }

        public T Find<T>(Expression<Func<T, bool>> where) where T : class
        {
          return  GetType<T>().FirstOrDefault(where);
            
        }

        public int Insert<T>(T obj) where T : class
        {
            GetType<T>().Add(obj);
            return Save();
        }

        public List<T> List<T>() where T : class
        {
            return GetType<T>().ToList();
        }

        public List<T> List<T>(Expression<Func<T, bool>> where) where T : class
        {
           return GetType<T>().Where(where).ToList();
        }

        public IQueryable<T> ListQueryable<T>() where T : class
        {
            return GetType<T>();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public int Update<T>(T obj) where T : class
        {
            return Save();
        }

        public Task<EntityEntry<T>> InsertAsync<T>(T obj) where T : class
        {
            return GetType<T>().AddAsync(obj);
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<T> FindAsync<T>(Expression<Func<T, bool>> where) where T : class
        {
            return GetType<T>().FirstOrDefaultAsync(where);
        }
    }
}
