using GoProcure.Domain.Common;
using GoProcure.Domain.Enums;
using GoProcure.Domain.Events;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class PurchaseOrder : AuditableEntity<Guid>
    {
        private readonly List<PurchaseOrderLine> _lines = new();
        public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

        public Guid PurchaseRequestId { get; private set; }
        public Guid VendorId { get; private set; }
        public POStatus Status { get; private set; } = POStatus.Draft;
        public int Version { get; private set; } = 1;
        public Money Total { get; private set; } = Money.Zero();

        private PurchaseOrder() { }

        public PurchaseOrder(Guid prId, Guid vendorId)
        {
            Id = Guid.NewGuid();
            PurchaseRequestId = prId != Guid.Empty ? prId : throw new DomainException("PR id required.");
            VendorId = vendorId != Guid.Empty ? vendorId : throw new DomainException("Vendor id required.");
            AddDomainEvent(new PurchaseOrderCreated(Id, prId, vendorId));
        }

        public void AddLine(Guid itemId, int qty, Money unitPrice)
        {
            if (qty <= 0) throw new DomainException("Quantity must be positive.");
            if (unitPrice.Amount <= 0) throw new DomainException("Unit price must be positive.");
            _lines.Add(new PurchaseOrderLine(Id, itemId, qty, unitPrice));
            Total = _lines.Aggregate(Money.Zero(), (acc, l) => acc.Add(l.Subtotal));
        }

        public void Send()
        {
            if (!_lines.Any()) throw new DomainException("PO needs at least one line.");
            Status = POStatus.Sent;
            AddDomainEvent(new PurchaseOrderSent(Id));
        }

        public PurchaseOrder Amend()
        {
            var amended = new PurchaseOrder
            {
                Id = Guid.NewGuid(),
                PurchaseRequestId = this.PurchaseRequestId,
                VendorId = this.VendorId,
                Version = this.Version + 1,
                Status = POStatus.Draft
            };
            foreach (var l in _lines)
                amended._lines.Add(new PurchaseOrderLine(amended.Id, l.ItemId, l.Qty, l.UnitPrice));
            amended.Total = amended._lines.Aggregate(Money.Zero(), (acc, l) => acc.Add(l.Subtotal));
            AddDomainEvent(new PurchaseOrderCreated(amended.Id, amended.PurchaseRequestId, amended.VendorId));
            return amended;
        }
    }
}
