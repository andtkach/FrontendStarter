using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Domain.Common;
using Jiwebapi.Catalog.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Jiwebapi.Catalog.Message
{
    public class MessageDbContext: DbContext
    {
        private readonly ILoggedInUserService? _loggedInUserService;
        
        public MessageDbContext(DbContextOptions<MessageDbContext> options, ILoggedInUserService loggedInUserService)
            : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }

        public DbSet<Log> Logs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessageDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
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

            foreach (var entry in ChangeTracker.Entries<IIdentifiableEntity>())
            {
                if (_loggedInUserService != null && !string.IsNullOrEmpty(_loggedInUserService.UserId))
                {
                    entry.Entity.UserId = Guid.Parse(_loggedInUserService.UserId);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
