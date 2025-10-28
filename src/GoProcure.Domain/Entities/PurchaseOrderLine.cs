using GoProcure.Domain.Common;
using GoProcure.Domain.ValueObjects;

namespace GoProcure.Domain.Entities
{
    public sealed class PurchaseOrderLine : AuditableEntity<Guid>
    {
        public Guid PurchaseOrderId { get; private set; }
        public Guid ItemId { get; private set; }
        public int Qty { get; private set; }
        public Money UnitPrice { get; private set; }
        public Money Subtotal => new(UnitPrice.Amount * Qty, UnitPrice.Currency);

        private PurchaseOrderLine() { }

        public PurchaseOrderLine(Guid poId, Guid itemId, int qty, Money unitPrice)
        {
            Id = Guid.NewGuid();
            PurchaseOrderId = poId; ItemId = itemId; Qty = qty; UnitPrice = unitPrice;
        }
    }
}
