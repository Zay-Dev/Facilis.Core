﻿using Facilis.Core.Abstractions;
using System;
using System.Linq;

namespace Facilis.ExtendedEntities.Abstractions
{
    public interface IExtendedAttributes
    {
        string Scope { get; }

        IQueryable<IExtendedAttribute> WhereEnabledDescendingSort(string scopedId, string key);

        bool AnyEnabled(string scopedId, string key);

        IExtendedAttribute AddNoSave(string scopedId, string key, string value);

        IExtendedAttribute AddOrUpdateNoSave(string scopedId, string key, string value);

        IExtendedAttribute UpdateNoSave(string id, string value);

        void Save();
    }

    public interface IExtendedAttributes<T> where T : IExtendedAttribute
    {
        IEntities<T> Entities { get; }

        T CreateEntity(string scopedId, string key, string value);
    }

    public class ExtendedAttributes<T> :
        IExtendedAttributes, IExtendedAttributes<T>
        where T : class, IExtendedAttribute, new()
    {
        private IEntities<T> entities { get; }
        private IOperators operators { get; }

        public string Scope { get; set; }
        public IEntities<T> Entities => this.entities;

        #region Constructor(s)

        public ExtendedAttributes(IEntities<T> entities, IOperators operators)
        {
            this.entities = entities;
            this.operators = operators;
        }

        #endregion Constructor(s)

        public virtual T CreateEntity(string scopedId, string key, string value)
        {
            return new T()
            {
                CreatedBy = this.operators.GetCurrentOperatorName(),
                UpdatedBy = this.operators.GetCurrentOperatorName(),
                Scope = this.Scope,
                ScopedId = scopedId,
                Key = key,
                Value = value
            };
        }

        public virtual IQueryable<IExtendedAttribute> WhereEnabledDescendingSort(
            string scopedId,
            string key
        )
        {
            return this.entities
                .WhereEnabled(entity => entity.Scope == this.Scope &&
                    entity.ScopedId == scopedId &&
                    entity.Key == key
                )
                .OrderByDescending(entity => entity.CreatedAtUtc);
        }

        public virtual bool AnyEnabled(string scopedId, string key)
        {
            return this.WhereEnabledDescendingSort(scopedId, key).Any();
        }

        public virtual IExtendedAttribute AddNoSave(string scopedId, string key, string value)
        {
            var entity = this.CreateEntity(scopedId, key, value);
            this.entities.AddNoSave(entity);

            return entity;
        }

        public virtual IExtendedAttribute UpdateNoSave(string id, string value)
        {
            var entity = this.entities.FindById(id);

            entity.UpdatedBy = this.operators.GetCurrentOperatorName();
            entity.UpdatedAtUtc = DateTime.UtcNow;
            entity.Value = value;

            this.entities.UpdateNoSave(entity);
            return entity;
        }

        public virtual IExtendedAttribute AddOrUpdateNoSave(string scopedId, string key, string value)
        {
            var entity = this.WhereEnabledDescendingSort(scopedId, key)
                .FirstOrDefault();

            return entity == null ?
                this.AddNoSave(scopedId, key, value) :
                this.UpdateNoSave(entity.Id, value);
        }

        public virtual void Save() => this.entities.Save();
    }

    public class ExtendedAttributes : ExtendedAttributes<ExtendedAttribute>
    {
        #region Constructor(s)

        public ExtendedAttributes(
            IEntities<ExtendedAttribute> entities,
            IOperators operators
        ) : base(entities, operators)
        {
        }

        #endregion Constructor(s)
    }
}