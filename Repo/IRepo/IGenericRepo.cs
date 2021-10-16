﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Repo
{
    public interface IGenericRepo<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetByExp(Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(int id);
        Task<bool> Create(TEntity entity);
        Task<bool> CreateRange(List<TEntity> entity);
        Task<bool> Update(TEntity entity);
        Task<bool> UpdateRange(List<TEntity> entity);
    }
}
