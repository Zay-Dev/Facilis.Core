using System.Linq;

namespace Facilis.Core.Abstractions
{
    public interface IScopedEntities<T> : IEntities<T>
        where T : IEntityWithId, IEntityWithStatus, IEntityWithScope
    {
        public string Scope { get; set; }

        public IScopedEntities<T> ChangeScope(string scope);

        IQueryable<T> QueryEnabledByScopedId(string scopedId);
    }
}