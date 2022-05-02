using Facilis.Core.Abstractions;
using Facilis.Core.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace Facilis.Core.Tests
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ExtendedAttribute> ExtendedAttributes { get; set; }

        #region Constructor(s)

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Constructor(s)
    }
}