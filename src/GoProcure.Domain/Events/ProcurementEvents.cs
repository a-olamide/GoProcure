using GoProcure.Domain.Common;
using GoProcure.Domain.Enums;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Events
{
    public sealed record PurchaseRequestCreated(Guid PrId, Guid RequesterId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record PurchaseRequestSubmitted(Guid PrId, Money Total) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record PurchaseRequestDecided(Guid PrId, Guid ApproverId, ApprovalDecision Decision) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record PurchaseOrderCreated(Guid PoId, Guid PrId, Guid VendorId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record PurchaseOrderSent(Guid PoId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record GoodsReceived(Guid PoId, Guid PoLineId, int Qty) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record InvoiceSubmitted(Guid InvoiceId, Guid PoId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record InvoiceMatched(Guid InvoiceId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    public sealed record PaymentRecorded(Guid PaymentId, Guid InvoiceId) : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }
}
