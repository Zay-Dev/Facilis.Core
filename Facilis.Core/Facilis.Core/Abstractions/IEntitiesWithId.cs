using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Facilis.Core.Abstractions
{
    public interface IEntitiesWithId<T> : IDisposable
        where T : IEntityWithId
    {
        IQueryable<T> Rows { get; }

        T FindById(string id);

        T[] FindByIds(params string[] ids);

        T[] FindByIds(IEnumerable<string> ids);

        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        T Add(T entity);

        T[] Add(params T[] entities);

        T[] Add(IEnumerable<T> entities);

        T Update(T entity);

        T[] Update(params T[] entities);

        T[] Update(IEnumerable<T> entities);

        void AddNoSave(params T[] entities);

        void AddNoSave(IEnumerable<T> entities);

        void UpdateNoSave(params T[] entities);

        void UpdateNoSave(IEnumerable<T> entities);

        void Save();
    }
}