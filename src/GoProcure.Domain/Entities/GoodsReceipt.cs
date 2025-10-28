using GoProcure.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class GoodsReceipt : AuditableEntity<Guid>
    {
        private readonly List<GoodsReceiptLine> _lines = new();
        public Guid PurchaseOrderId { get; private set; }
        public DateTime ReceivedDateUtc { get; private set; } = DateTime.UtcNow;
        public IReadOnlyCollection<GoodsReceiptLine> Lines => _lines.AsReadOnly();

        private GoodsReceipt() { }
        public GoodsReceipt(Guid poId)
        {
            Id = Guid.NewGuid();
            PurchaseOrderId = poId != Guid.Empty ? poId : throw new DomainException("PO id required.");
        }

        public void AddLine(Guid poLineId, int qtyReceived, int qtyDamaged = 0, string? notes = null)
        {
            if (qtyReceived < 0 || qtyDamaged < 0) throw new DomainException("Quantities cannot be negative.");
            _lines.Add(new GoodsReceiptLine(Id, poLineId, qtyReceived, qtyDamaged, notes));
        }
    }
}
