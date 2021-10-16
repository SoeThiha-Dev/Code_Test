using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.Model;

namespace WebAPI.Repo
{
    public class GenericRepo<TEntity> : IGenericRepo<TEntity>
        where TEntity : class
    {
        private readonly WebAPIDBContext _dbContext;
        public GenericRepo(WebAPIDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _dbContext.Set<TEntity>().AsNoTracking();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<TEntity> GetByExp(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _dbContext.Set<TEntity>().Where(predicate).AsNoTracking();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TEntity GetById(int id)
        {
            try
            {
                return _dbContext.Set<TEntity>().Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Create(TEntity entity)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> CreateRange(List<TEntity> entity)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddRangeAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Update(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Update(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateRange(List<TEntity> entity)
        {
            try
            {
                _dbContext.Set<TEntity>().UpdateRange(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
