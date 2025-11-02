using GoProcure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.Persistence.Configurations
{
    public sealed class ItemConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> b)
        {
            b.ToTable("Items");
            b.HasKey(x => x.Id);

            b.OwnsOne(x => x.DefaultPrice, owned =>
            {
                owned.Property(p => p.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)");

                owned.Property(p => p.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(20);
            });
        }
    }
}
