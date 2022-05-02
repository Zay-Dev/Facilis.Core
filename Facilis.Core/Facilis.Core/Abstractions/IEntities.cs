using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Facilis.Core.Abstractions
{
    public interface IEntities<T> : IEntitiesWithId<T>
        where T : IEntityWithId, IEntityWithStatus
    {
        T FindEnabledById(string id);

        IQueryable<T> WhereEnabledByIds(params string[] ids);

        IQueryable<T> WhereEnabledByIds(IEnumerable<string> ids);

        IQueryable<T> WhereEnabled(Expression<Func<T, bool>> expression = null);

        IQueryable<T> WhereAll(Expression<Func<T, bool>> expression, bool expectDeleted = false);
    }
}