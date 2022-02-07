using Fin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fin.Infrastructure
{
    public class FinDbContext : DbContext
    {
        public Guid UserId { get; set; }

        public FinDbContext() : base() { }

        public FinDbContext(DbContextOptions<FinDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        
        public virtual DbSet<Trade> Trades { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps()
        {
            IEnumerable<EntityEntry> entities = ChangeTracker.Entries()
                .Where(x => (x.Entity is BaseEntity) && (x.State == EntityState.Added || x.State == EntityState.Modified));

            DateTime now = DateTime.Now;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).Created = now;
                }

                ((BaseEntity)entity.Entity).Modified = now;
            }
        }
    }
}
