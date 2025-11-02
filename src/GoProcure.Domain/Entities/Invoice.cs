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
    public sealed class Invoice : AuditableEntity<Guid>
    {
        private readonly List<InvoiceLine> _lines = new();
        public Guid PurchaseOrderId { get; private set; }
        public string VendorInvoiceNo { get; private set; } = default!;
        public DateTime InvoiceDateUtc { get; private set; }
        public InvoiceStatus Status { get; private set; } = InvoiceStatus.Submitted;
        public Money Total { get; private set; } = Money.Zero();

        public IReadOnlyCollection<InvoiceLine> Lines => _lines.AsReadOnly();

        private Invoice() { }
        public Invoice(Guid poId, string vendorInvoiceNo, DateTime dateUtc)
        {
            Id = Guid.NewGuid();
            PurchaseOrderId = poId != Guid.Empty ? poId : throw new DomainException("PO id required.");
            VendorInvoiceNo = string.IsNullOrWhiteSpace(vendorInvoiceNo) ? throw new DomainException("Invoice no required.") : vendorInvoiceNo.Trim();
            InvoiceDateUtc = dateUtc;
            AddDomainEvent(new InvoiceSubmitted(Id, poId));
        }

        public void AddLine(Guid poLineId, int qty, Money unitPrice)
        {
            if (qty <= 0 || unitPrice.Amount <= 0) throw new DomainException("Invalid invoice line.");
            _lines.Add(new InvoiceLine(Id, poLineId, qty, unitPrice));
            Total = _lines.Aggregate(Money.Zero(), (acc, l) => acc.Add(l.Subtotal));
        }

        public void MarkMatched() { Status = InvoiceStatus.Matched; AddDomainEvent(new InvoiceMatched(Id)); }
        public void MarkMismatch() { Status = InvoiceStatus.Mismatch; }
    }
}
