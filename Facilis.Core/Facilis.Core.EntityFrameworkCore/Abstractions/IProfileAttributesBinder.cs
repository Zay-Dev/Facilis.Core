using Facilis.Core.Abstractions;
using Facilis.Core.Attributes;
using Facilis.Core.EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Facilis.Core.EntityFrameworkCore.Abstractions
{
    public interface IProfileAttributesBinder
    {
        bool HasPendingChanges();

        void DbContextSavedChanges(object sender, SavedChangesEventArgs e);

        void DbContextSavingChanges(object sender, SavingChangesEventArgs e);
    }

    public class ProfileAttributesBinder<T> : IProfileAttributesBinder
        where T : class, IExtendedAttribute, new()
    {
        private List<BindingProfileAttributes<T>> bindingModels { get; } = new();

        private IScopeBuilder scopeBuilder { get; }
        private IEntityStampsBinder entityStampsBinder { get; }

        #region Constructor(s)

        public ProfileAttributesBinder(
            IScopeBuilder scopeBuilder,
            IEntityStampsBinder entityStampsBinder
        )
        {
            this.scopeBuilder = scopeBuilder;
            this.entityStampsBinder = entityStampsBinder;
        }

        #endregion Constructor(s)

        public virtual bool HasPendingChanges()
        {
            return this.bindingModels.Count > 0;
        }

        public virtual void DbContextSavingChanges(object sender, SavingChangesEventArgs e)
        {
            var trackStates = new[]
            {
                EntityState.Added,
                EntityState.Modified,
            };

            if (sender is DbContext context)
            {
                this.Track(
                    context,
                    context.ChangeTracker
                        .Entries()
                        .Where(entry => trackStates.Contains(entry.State))
                );
            }
        }

        public virtual void DbContextSavedChanges(object sender, SavedChangesEventArgs e)
        {
            if (this.HasPendingChanges() && sender is DbContext context)
            {
                var entities = this.GetScopedEntities(context);

                var newAttributes = new List<T>();
                var updatingAttributes = new List<T>();

                foreach (var model in this.bindingModels)
                {
                    newAttributes.AddRange(model.GetNewAttributes());
                    updatingAttributes.AddRange(model.GetUpdatingAttributes());
                }

                if (newAttributes.Any())
                {
                    entities.AddNoSave(newAttributes);
                }

                if (updatingAttributes.Any())
                {
                    entities.UpdateNoSave(updatingAttributes);
                }

                this.Clear();
                entities.Save();
            }
        }

        protected virtual void Track(DbContext context, IEnumerable<EntityEntry> entries)
        {
            var entities = this.GetScopedEntities(context);

            foreach (var tracked in entries)
            {
                if (tracked.Entity is IEntityWithProfile entity)
                {
                    var profile = entity.UncastedProfile;
                    if (profile == null) continue;

                    var scope = this.scopeBuilder.GetScopeOf(entity);
                    var scopedId = ((IEntityWithId)entity).Id;
                    var profileAttributes = this.GetAttributes(entities, entity, scope, scopedId);

                    foreach (var property in profile.GetType().GetProperties())
                    {
                        var key = property.Name;
                        var rawValue = ToRawValue(profile, property);

                        if (property.GetCustomAttribute(typeof(ImmutableAttribute)) != null)
                        {
                            profileAttributes.ImmutableKeys.Add(key);
                        }
                        profileAttributes.ValuesGroupedInKeys.Add(key, rawValue);
                    }

                    this.bindingModels.Add(profileAttributes);
                }
            }
        }

        protected virtual IScopedEntities<T> GetScopedEntities(DbContext context)
        {
            return new ScopedEntities<T>(context);
        }

        private BindingProfileAttributes<T> GetAttributes(
            IScopedEntities<T> entities,
            IEntityWithProfile entity,
            string scope,
            string scopedId
        )
        {
            return new BindingProfileAttributes<T>()
            {
                EntityId = scopedId,
                EntityStatus = ((IEntityWithStatus)entity).Status,

                Scope = scope,
                Attributes = entities
                    .ChangeScope(scope)
                    .QueryEnabledByScopedId(scopedId)
                    .ToArray(),

                EntityStampsBinder = this.entityStampsBinder,
            };
        }

        private void Clear()
        {
            this.bindingModels.Clear();
        }

        private static string ToRawValue(object profile, PropertyInfo property)
        {
            var value = property.GetValue(profile, null);
            var rawValue = value == null ? null : JsonSerializer.Serialize(value);

            return property.PropertyType == typeof(string) && value != null ?
                rawValue[1..^1] :
                rawValue;
        }
    }

    public class ProfileAttributesBinder
        : ProfileAttributesBinder<ExtendedAttribute>
    {
        #region Constructor(s)

        public ProfileAttributesBinder(
            IScopeBuilder scopeBuilder,
            IEntityStampsBinder entityStampsBinder
        ) : base(scopeBuilder, entityStampsBinder)
        {
        }

        #endregion Constructor(s)
    }
}