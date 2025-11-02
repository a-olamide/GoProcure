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

    public sealed class VendorConfig : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> b)
        {
            b.ToTable("Vendors");
            b.HasKey(x => x.Id);

            // Owned: BankAccountVO → columns on Vendors table
            b.OwnsOne(x => x.Bank, owned =>
            {
                owned.Property(p => p.BankName)
                     .HasColumnName("BankName")
                     .HasMaxLength(128);

                owned.Property(p => p.AccountName)
                     .HasColumnName("BankAccountName")
                     .HasMaxLength(128);

                owned.Property(p => p.AccountNumber)
                     .HasColumnName("BankAccountNumber")
                     .HasMaxLength(34)
                     .IsUnicode(false);

                // optional indexes on owned columns:
                // owned.HasIndex(p => p.AccountNumber);
            });

            b.OwnsOne(x => x.Email, owned =>
            {
                owned.Property(p => p.Value)
                .HasColumnName("Email")
                .HasMaxLength(120);
            });
            b.OwnsOne(x => x.Phone, owned =>
            {
                owned.Property(p => p.Value)
                .HasColumnName("Phone")
                .HasMaxLength(120);
            });

        }
    }
}
