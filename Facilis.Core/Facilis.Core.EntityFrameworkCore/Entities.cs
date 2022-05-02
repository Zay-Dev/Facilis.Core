using Facilis.Core.Abstractions;
using Facilis.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Facilis.Core.EntityFrameworkCore
{
    public class Entities<T> : EntitiesWithId<T>, IEntities<T>
        where T : class, IEntityWithId, IEntityWithStatus
    {
        #region Constructor(s)

        public Entities(DbContext context) : base(context)
        {
        }

        #endregion Constructor(s)

        public T FindEnabledById(string id)
        {
            return this.Rows.FirstOrDefault(entity => entity.Id == id &&
                entity.Status == StatusTypes.Enabled
            );
        }

        public IQueryable<T> WhereEnabledByIds(params string[] ids)
        {
            return this.WhereEnabled(entity => ids.Contains(entity.Id));
        }

        public IQueryable<T> WhereEnabledByIds(IEnumerable<string> ids)
        {
            return this.WhereEnabledByIds(ids.ToArray());
        }

        public IQueryable<T> WhereAll(Expression<Func<T, bool>> expression, bool expectDeleted = false)
        {
            var entities = expression == null ?
                this.Rows : base.Where(expression);

            return expectDeleted ?
                entities :
                entities.Where(entity => entity.Status != StatusTypes.Deleted);
        }

        public IQueryable<T> WhereEnabled(Expression<Func<T, bool>> expression = null)
        {
            var entities = this.Rows
                .Where(entity => entity.Status == StatusTypes.Enabled);

            return expression == null ? entities : entities.Where(expression);
        }
    }
}