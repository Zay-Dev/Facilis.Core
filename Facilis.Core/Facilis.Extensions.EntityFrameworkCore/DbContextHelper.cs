using Facilis.Core.Abstractions;
using Facilis.Core.EntityFrameworkCore;
using Facilis.Core.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Facilis.Extensions.EntityFrameworkCore
{
    public static class DbContextHelper
    {
        public static IServiceCollection AddDefaultEntities(
            this IServiceCollection services
        )
        {
            return services
                .AddScoped(typeof(IEntities<>), typeof(Entities<>))
                .AddScoped(typeof(IEntitiesWithId<>), typeof(EntitiesWithId<>))
                .AddScoped(typeof(IScopedEntities<>), typeof(ScopedEntities<>));
        }

        public static IServiceCollection AddProfileAttributesBinder<T>(
            this IServiceCollection services
        ) where T : class, IProfileAttributesBinder
        {
            return services.AddScoped<IProfileAttributesBinder, T>();
        }

        public static IServiceCollection AddDefaultProfileAttributesBinder(
            this IServiceCollection services
        )
        {
            return services.AddProfileAttributesBinder<ProfileAttributesBinder>();
        }

        public static void UseProfileAttributesBinder(
            this DbContext context,
            IProfileAttributesBinder binder
        )
        {
            context.SavingChanges += binder.DbContextSavingChanges;
            context.SavedChanges += binder.DbContextSavedChanges;
        }

        public static async Task UseProfileAttributesBinder(
            this IServiceProvider provider,
            Func<Task> next
        )
        {
            provider.GetRequiredService<DbContext>()
                .UseProfileAttributesBinder(provider
                    .GetRequiredService<IProfileAttributesBinder>()
                );
            await next();
        }
    }
}