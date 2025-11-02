using GoProcure.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.Outbox
{
    public sealed class OutboxMessage : Entity<Guid>
    {
        public string Type { get; private set; } = default!;
        public string Payload { get; private set; } = default!;
        public DateTime OccurredOnUtc { get; private set; }
        public DateTime? ProcessedOnUtc { get; private set; }
        public string? Error { get; private set; }

        private OutboxMessage() { }

        public static OutboxMessage From(IDomainEvent de) => new()
        {
            Id = Guid.NewGuid(),
            Type = de.GetType().FullName ?? de.GetType().Name,
            Payload = JsonSerializer.Serialize(de),
            OccurredOnUtc = de.OccurredOnUtc
        };

        public void MarkProcessed() => ProcessedOnUtc = DateTime.UtcNow;
        public void MarkFailed(string error) => Error = error;
    }
}
