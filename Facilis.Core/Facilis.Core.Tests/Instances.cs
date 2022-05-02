using Facilis.Core.Abstractions;
using Facilis.Core.EntityFrameworkCore;
using Facilis.Core.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Facilis.Core.Tests
{
    public class Instances : IDisposable
    {
        public DbContext Context { get; }

        #region Constructor(s)

        public Instances()
        {
            this.Context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: nameof(Facilis))
                    .Options
            );

            AutoBindProfile(this.Context);
        }

        #endregion Constructor(s)

        public IEntities<T> GetEntities<T>()
            where T : class, IEntityWithId, IEntityWithStatus
        {
            return new Entities<T>(this.Context);
        }

        public void Dispose()
        {
            this.Context?.Database.EnsureDeleted();
            this.Context?.Dispose();
        }

        private static void AutoBindProfile(DbContext context)
        {
            var entityStampsBinder = new EntityStampsBinder()
            {
                CurrentUserIdentifier = nameof(Facilis),
                SystemOperatorIdentifier = nameof(System),
            };
            var attributes = new ScopedEntities<ExtendedAttribute>(context);
            var profileBinder = new ProfileAttributesBinder(
                new ScopeBuilder(),
                entityStampsBinder
            );

            context.SavingChanges += profileBinder.DbContextSavingChanges;
            context.SavedChanges += profileBinder.DbContextSavedChanges;
        }
    }
}