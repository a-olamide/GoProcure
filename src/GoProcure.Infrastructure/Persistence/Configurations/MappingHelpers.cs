using GoProcure.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.Persistence.Configurations
{
    internal static class MappingHelpers
    {
        public static void MapMoney(this OwnedNavigationBuilder<object, Money> owned, string prefix)
        {
            owned.Property(p => p.Amount).HasColumnName($"{prefix}Amount").HasColumnType("decimal(18,2)");
            owned.Property(p => p.Currency).HasColumnName($"{prefix}Currency").HasMaxLength(3).IsUnicode(false);
        }

        public static void MapEmail(this OwnedNavigationBuilder<object, Email> owned, string columnName = "Email")
        {
            owned.Property(p => p.Value).HasColumnName(columnName).HasMaxLength(256);
        }

        public static void MapPhone(this OwnedNavigationBuilder<object, PhoneNumber> owned, string columnName = "Phone")
        {
            owned.Property(p => p.Value).HasColumnName(columnName).HasMaxLength(20).IsUnicode(false);
            owned.HasIndex(p => p.Value); // optional: for lookups
        }

        public static void MapBankAccount(this OwnedNavigationBuilder<object, BankAccountVO> owned, string prefix = "Bank")
        {
            owned.Property(p => p.BankName).HasColumnName($"{prefix}Name").HasMaxLength(128);
            owned.Property(p => p.AccountName).HasColumnName($"{prefix}AccountName").HasMaxLength(128);
            owned.Property(p => p.AccountNumber).HasColumnName($"{prefix}AccountNumber").HasMaxLength(34).IsUnicode(false);
            owned.HasIndex(p => p.AccountNumber); // optional
        }
    }
}
