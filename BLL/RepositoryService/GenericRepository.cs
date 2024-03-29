﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Extensions;
namespace BLL.RepositoryService
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> _entities;

        public GenericRepository(DbContext context)
        {
            Context = context;
            _entities = context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            return _entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void Edit(TEntity entity)
        {
            _entities.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TEntity entity)
        {
            _entities.Attach(entity);
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public TEntity Get(int id)
        {
            return _entities.Find(id);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.AsNoTracking().SingleOrDefault(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.AsNoTracking().FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities.AsNoTracking().ToList();
        }

        public int GetRecordsNumber()
        {
            return _entities.Count();
        }

        public int GetRecordsNumber(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate).Count();
        }

        public void Remove(Expression<Func<TEntity, bool>> predicate)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _entities.Delete(predicate);
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
