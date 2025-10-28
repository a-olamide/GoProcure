using GoProcure.Domain.Common;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class InvoiceLine : AuditableEntity<Guid>
    {
        public Guid InvoiceId { get; private set; }
        public Guid PurchaseOrderLineId { get; private set; }
        public int Qty { get; private set; }
        public Money UnitPrice { get; private set; }
        public Money Subtotal => new(UnitPrice.Amount * Qty, UnitPrice.Currency);
        private InvoiceLine() { }
        public InvoiceLine(Guid invoiceId, Guid poLineId, int qty, Money unitPrice)
        {
            Id = Guid.NewGuid();
            InvoiceId = invoiceId; PurchaseOrderLineId = poLineId; Qty = qty; UnitPrice = unitPrice;
        }
    }
}
