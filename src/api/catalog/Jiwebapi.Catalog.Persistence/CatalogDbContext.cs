using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Domain.Common;
using Jiwebapi.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jiwebapi.Catalog.Persistence
{
    public class CatalogDbContext: DbContext
    {
        private readonly ILoggedInUserService? _loggedInUserService;
        
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options, ILoggedInUserService loggedInUserService)
            : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);

            var category = Guid.Parse("{10788D2F-8003-43C1-92A4-EDC76A7C5DDE}");
            
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = category,
                Name = "Demo"
            });
            
            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{1E272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                Name = "Content engine release",
                Person = "Andrii Tkach",
                Date = DateTime.UtcNow.AddDays(9),
                Description = "Content engine application release v3.1 with admin portal and mobile app.",
                ImageUrl = "https://someimage.jpg",
                CategoryId = category
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _loggedInUserService?.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        entry.Entity.ModifiedBy = _loggedInUserService?.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
