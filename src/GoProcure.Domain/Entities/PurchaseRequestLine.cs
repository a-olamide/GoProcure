using GoProcure.Domain.Common;
using GoProcure.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace GoProcure.Domain.Entities
{
    public sealed class PurchaseRequestLine : AuditableEntity<Guid>
    {
        public Guid PurchaseRequestId { get; private set; }
        public Guid ItemId { get; private set; }
        [MaxLength(2000)]
        public string Description { get; private set; } = default!;
        public int Qty { get; private set; }
        public Money UnitPrice { get; private set; }
        public Money Subtotal => new(UnitPrice.Amount * Qty, UnitPrice.Currency);
        public Guid? SuggestedVendorId { get; private set; }

        private PurchaseRequestLine() { }

        public PurchaseRequestLine(Guid prId, Guid itemId, string description, int qty, Money unitPrice, Guid? suggestedVendorId)
        {
            Id = Guid.NewGuid();
            PurchaseRequestId = prId;
            ItemId = itemId;
            Description = (description ?? string.Empty).Trim();
            Qty = qty;
            UnitPrice = unitPrice;
            SuggestedVendorId = suggestedVendorId;
        }

        public void Update(int qty, Money unitPrice, Guid? suggestedVendorId)
        {
            Qty = qty;
            UnitPrice = unitPrice;
            SuggestedVendorId = suggestedVendorId;
        }
    }
}
