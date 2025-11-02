using GoProcure.Domain.Common;
using GoProcure.Domain.Entities;
using GoProcure.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.Persistence
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Domain DbSets (only used inside Infrastructure)
        public DbSet<Vendor> Vendors => Set<Vendor>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<VendorItem> VendorItems => Set<VendorItem>();
        public DbSet<PurchaseRequest> PurchaseRequests => Set<PurchaseRequest>();
        public DbSet<PurchaseRequestLine> PurchaseRequestLines => Set<PurchaseRequestLine>();
        public DbSet<Approval> Approvals => Set<Approval>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
        public DbSet<GoodsReceipt> GoodsReceipts => Set<GoodsReceipt>();
        public DbSet<GoodsReceiptLine> GoodsReceiptLines => Set<GoodsReceiptLine>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
        public DbSet<Payment> Payments => Set<Payment>();

        // Outbox stays internal to Infrastructure
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Owned<GoProcure.Domain.ValueObjects.BankAccountVO>();
            b.Owned<GoProcure.Domain.ValueObjects.Email>();
            b.Owned<GoProcure.Domain.ValueObjects.PhoneNumber>();
            b.Owned<GoProcure.Domain.ValueObjects.Money>();


            // Apply all IEntityTypeConfiguration<T> from this assembly (your fluent configs live here)
            b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Global soft-delete filter for AuditableEntity<T>
            foreach (var et in b.Model.GetEntityTypes())
            {
                var clr = et.ClrType;
                var baseType = clr.BaseType;
                if (baseType is null) continue;

                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(AuditableEntity<>))
                {
                    var param = Expression.Parameter(clr, "e");
                    var prop = Expression.Property(param, nameof(AuditableEntity<Guid>.IsDeleted));
                    var body = Expression.Equal(prop, Expression.Constant(false));
                    var lambda = Expression.Lambda(body, param);
                    et.SetQueryFilter(lambda);
                }
            }
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            // Default all strings to nvarchar(256)
            builder.Properties<string>().HaveMaxLength(256).AreUnicode(true);

            // Default decimals (e.g., money) to decimal(18,2)
            builder.Properties<decimal>().HavePrecision(18, 2);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            // audit timestamps
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditableEntity<Guid> aud)
                {
                    if (entry.State == EntityState.Added) aud.SetCreatedAtUtc();
                    if (entry.State == EntityState.Modified) aud.SetModifiedAtUtc();
                }
            }

            // Gather domain events BEFORE saving (for transactional outbox)
            var domainEvents = ChangeTracker.Entries<Entity<Guid>>()
                .SelectMany(e => e.Entity.DequeueDomainEvents())
                .ToList();

            foreach (var de in domainEvents)
                OutboxMessages.Add(OutboxMessage.From(de));

            // Single atomic commit (data + outbox)
            return await base.SaveChangesAsync(ct);
        }
    }
}
