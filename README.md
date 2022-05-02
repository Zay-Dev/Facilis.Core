### Facilis.Core

Dependency-less core library, mostly contains the abstract and enums elements


### Facilis.Core.EntityFrameworkCore

Entity Framework Core's implementations


### Entities

#### IEntityWithId

The basic interface to provide a `string Id` property


#### IEntityWithStatus

The interface that provide a `Status` property of enum `StatusTypes`

** Models that implemented the `IEntityWithId` only can be accessed by the `IEntitiesWithId`, models also implemented the `IEntityWithStatus` can be accessed by the `IEntities`


#### IEntityWithScope

The interface that provide the `string Scope` and `string ScopedId` properties

** Models that implemented the `IEntityWithScope` can be access by the `IEntitiesWithScope`


#### IScopeBuilder

The interface to build the scope by an object or a type


#### IUserRelatedEntity

The interface that provide the `string UserId` property


#### IEntityWithCreateStamps

The interface that provide the `CreatedBy` and `CreatedAtUtc` properties


#### IEntityWithUpdateStamps

The interface that provide the `UpdatedBy` and `UpdatedAtUtc` properties

** `IEntityStampsBinder` is applicable to any models that implemented either the `IEntityWithCreateStamps` or `IEntityWithUpdateStampe`


#### IEntityWithProfile / IEntityWithProfile<T>

The interfaces that provide the `object UncastedProfile` and `T Profile` properties

** `IProfileAttributesBinder` in the EF Core is applicable to any models that implemented the `IEntityWithProfile` or `IEntityWithProfile<T>`


#### ISortableEntity

The interface that provide the `int AscendingOrder` property


### Injections


#### `IEntities` and `IEntitiesWithId`

```csharp
services
    .AddDbContext<AppDbContext>()
    .AddDbContext<DbContext, AppDbContext>()
    .AddDefaultEntities(); // -- the default entity framework core implementation
```


#### `IScopeBuilder`

```csharp
services.AddSingleton<IScopeBuilder, ScopeBuilder>(); // -- the default implementation
```


#### `IProfileAttributesBinder`

```csharp
services.AddDefaultProfileAttributesBinder(); // -- the default implementation

app.Use(async (context, next) =>
    await context.RequestServices.UseProfileAttributesBinder(next)
);
```


#### `IEntityStampsBinder`

```csharp
services
    .AddHttpContextAccessor()
    .AddScoped<IEntityStampsBinder>(provider =>
        new EntityStampsBinder()
        {
            SystemOperatorIdentifier = nameof(System),
            CurrentUserIdentifier = provider
                .GetRequiredService<IHttpContextAccessor>()
                .HttpContext.User.Identity.Name
        }
    );
```


### Stringify enum columns

```csharp
public class AppDbContext : DbContext
{
    ...

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ...
        this.UseStringifyEnumColumns(builder);
        ...
    }
}
```