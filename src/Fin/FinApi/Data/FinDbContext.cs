﻿using FinApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinApi
{
    public class FinDbContext : DbContext
    {
        public Guid UserId { get; set; }

        public FinDbContext(DbContextOptions<FinDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

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
            var entities = ChangeTracker.Entries()
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
