using System;

namespace Facilis.Core.Abstractions
{
    public interface IEntityStampsBinder
    {
        string SystemOperatorIdentifier { get; }
        string CurrentUserIdentifier { get; }

        void BindCreatedBySystem(IEntityWithCreateStamps entity);

        void BindCreatedByUser(IEntityWithCreateStamps entity);

        void BindUpdatedBySystem(IEntityWithUpdateStamps entity);

        void BindUpdatedByUser(IEntityWithUpdateStamps entity);

        T BindCreatedBySystem<T>(T entity)
            where T : IEntityWithCreateStamps, IEntityWithUpdateStamps;

        T BindCreatedByUser<T>(T entity)
            where T : IEntityWithCreateStamps, IEntityWithUpdateStamps;
    }

    public class EntityStampsBinder : IEntityStampsBinder
    {
        public string SystemOperatorIdentifier { get; set; }
        public string CurrentUserIdentifier { get; set; }

        public void BindCreatedBySystem(IEntityWithCreateStamps entity)
        {
            entity.CreatedAtUtc = DateTime.UtcNow;
            entity.CreatedBy = this.SystemOperatorIdentifier;
        }

        public void BindUpdatedBySystem(IEntityWithUpdateStamps entity)
        {
            entity.UpdatedAtUtc = DateTime.UtcNow;
            entity.UpdatedBy = this.SystemOperatorIdentifier;
        }

        public T BindCreatedBySystem<T>(T entity)
            where T : IEntityWithCreateStamps, IEntityWithUpdateStamps
        {
            this.BindCreatedBySystem((IEntityWithCreateStamps)entity);
            this.BindUpdatedBySystem(entity);

            return entity;
        }

        public void BindCreatedByUser(IEntityWithCreateStamps entity)
        {
            entity.CreatedAtUtc = DateTime.UtcNow;
            entity.CreatedBy = this.CurrentUserIdentifier;
        }

        public void BindUpdatedByUser(IEntityWithUpdateStamps entity)
        {
            entity.UpdatedAtUtc = DateTime.UtcNow;
            entity.UpdatedBy = this.CurrentUserIdentifier;
        }

        public T BindCreatedByUser<T>(T entity)
            where T : IEntityWithCreateStamps, IEntityWithUpdateStamps
        {
            this.BindCreatedByUser((IEntityWithCreateStamps)entity);
            this.BindUpdatedByUser(entity);

            return entity;
        }
    }
}