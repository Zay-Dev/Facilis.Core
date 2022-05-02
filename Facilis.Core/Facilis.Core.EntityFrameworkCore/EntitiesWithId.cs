using Facilis.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Facilis.Core.EntityFrameworkCore
{
    public class EntitiesWithId<T> : IEntitiesWithId<T>
        where T : class, IEntityWithId
    {
        private DbContext context { get; }

        protected virtual DbSet<T> Set => this.context.Set<T>();

        public virtual IQueryable<T> Rows => this.Set;

        #region Constructor(s)

        public EntitiesWithId(DbContext context)
        {
            this.context = context;
        }

        #endregion Constructor(s)

        public virtual T Add(T entity)
        {
            this.Set.Add(entity);
            this.context.SaveChanges();
            return entity;
        }

        public virtual T[] Add(params T[] entities)
        {
            this.Set.AddRange(entities);
            this.context.SaveChanges();
            return entities;
        }

        public virtual T[] Add(IEnumerable<T> entities)
        {
            return this.Add(entities.ToArray());
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return this.Rows.Where(expression);
        }

        public virtual void AddNoSave(params T[] entities)
        {
            this.Set.AddRange(entities);
        }

        public virtual void AddNoSave(IEnumerable<T> entities)
        {
            this.AddNoSave(entities.ToArray());
        }

        public virtual T FindById(string id)
        {
            return this.Rows
                .FirstOrDefault(entity => entity.Id == id);
        }

        public virtual T[] FindByIds(params string[] ids)
        {
            return this.Rows.Where(entity => ids.Contains(entity.Id)).ToArray();
        }

        public virtual T[] FindByIds(IEnumerable<string> ids)
        {
            return this.FindByIds(ids.ToArray());
        }

        public virtual void Save()
        {
            this.context.SaveChanges();
        }

        public virtual T Update(T entity)
        {
            this.Set.Update(entity);
            this.context.SaveChanges();

            return entity;
        }

        public virtual T[] Update(params T[] entities)
        {
            this.Set.UpdateRange(entities);
            this.context.SaveChanges();

            return entities;
        }

        public virtual T[] Update(IEnumerable<T> entities)
        {
            return this.Update(entities.ToArray());
        }

        public virtual void UpdateNoSave(params T[] entities)
        {
            this.Set.UpdateRange(entities);
        }

        public virtual void UpdateNoSave(IEnumerable<T> entities)
        {
            this.UpdateNoSave(entities.ToArray());
        }

        public virtual void Dispose()
        {
            this.context.Dispose();
        }
    }
}