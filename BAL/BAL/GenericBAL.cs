using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Repo;

namespace WebAPI.BAL
{
    public class GenericBAL<TEntity> : IGenericBAL<TEntity>
        where TEntity : class
    {

        private readonly IGenericRepo<TEntity> _genericRepo;
        public GenericBAL(IGenericRepo<TEntity> genericRepo)
        {
            _genericRepo = genericRepo;
        }

        public List<TEntity> GetAll()
        {
            try
            {
                return _genericRepo.GetAll().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TEntity> GetByExp(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _genericRepo.GetByExp(predicate).ToList();
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
                return _genericRepo.GetById(id);
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
                return await _genericRepo.Create(entity);
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
                return await _genericRepo.CreateRange(entity);
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
                return await _genericRepo.Update(entity);
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
                return await _genericRepo.UpdateRange(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
