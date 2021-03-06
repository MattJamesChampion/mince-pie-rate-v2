﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MincePieRateV2.Models.Domain;
using MincePieRateV2.Models.Metadata;

namespace MincePieRateV2.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<MincePie> MincePies { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public Guid UserId;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            UpdateModelMetadata();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateModelMetadata();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void UpdateModelMetadata()
        {
            //https://www.entityframeworktutorial.net/faq/set-created-and-modified-date-in-efcore.aspx
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ModelMetadata && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                ((ModelMetadata)entry.Entity).ModifiedBy = UserId;
                ((ModelMetadata)entry.Entity).ModifiedAt = DateTimeOffset.Now;

                if (entry.State == EntityState.Added)
                {
                    ((ModelMetadata)entry.Entity).CreatedBy = UserId;
                    ((ModelMetadata)entry.Entity).CreatedAt = DateTimeOffset.Now;
                }
            }
        }
    }
}
