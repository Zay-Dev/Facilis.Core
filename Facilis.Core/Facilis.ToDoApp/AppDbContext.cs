using Facilis.Core.Abstractions;
using Facilis.Core.EntityFrameworkCore.Helpers;
using Facilis.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Facilis.ToDoApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }

        #region Constructor(s)

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Constructor(s)

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            this.UseStringifyEnumColumns(builder);
        }
    }

    public class ToDoItem : IEntityWithId, IEntityWithStatus
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public StatusTypes Status { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}