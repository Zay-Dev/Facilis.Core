using Facilis.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Facilis.Core.EntityFrameworkCore
{
    public class ScopedEntities<T> : Entities<T>, IScopedEntities<T>
        where T : class, IEntityWithId, IEntityWithStatus, IEntityWithScope
    {
        private DbContext context { get; }

        public string Scope { get; set; }

        public override IQueryable<T> Rows => base.Rows
            .Where(row => row.Scope == this.Scope);

        #region Constructor(s)

        public ScopedEntities(DbContext context) : base(context)
        {
            this.context = context;
        }

        #endregion Constructor(s)

        public virtual IScopedEntities<T> ChangeScope(string scope)
        {
            return new ScopedEntities<T>(this.context)
            {
                Scope = scope
            };
        }

        public virtual IQueryable<T> QueryEnabledByScopedId(string scopedId)
        {
            return this.Rows.Where(row => row.ScopedId == scopedId);
        }
    }
}