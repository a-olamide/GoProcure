using GoProcure.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.Persistence.Configurations
{
    public sealed class OutboxConfig : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> b)
        {
            b.ToTable("OutboxMessages");
            b.HasKey(x => x.Id);
            b.Property(x => x.Type).HasMaxLength(250).IsRequired();
            b.Property(x => x.Payload).HasColumnType("nvarchar(max)").IsRequired();
            b.HasIndex(x => x.ProcessedOnUtc);
        }
    }
}
