using GoProcure.Domain.Common;

namespace GoProcure.Domain.Entities
{
    public sealed class GoodsReceiptLine : AuditableEntity<Guid>
    {
        public Guid GoodsReceiptId { get; private set; }
        public Guid PurchaseOrderLineId { get; private set; }
        public int QtyReceived { get; private set; }
        public int QtyDamaged { get; private set; }
        public string? Notes { get; private set; }

        private GoodsReceiptLine() { }
        public GoodsReceiptLine(Guid grId, Guid poLineId, int qtyReceived, int qtyDamaged, string? notes)
        {
            Id = Guid.NewGuid();
            GoodsReceiptId = grId; PurchaseOrderLineId = poLineId;
            QtyReceived = qtyReceived; QtyDamaged = qtyDamaged; Notes = notes?.Trim();
        }
    }
}
