using GoProcure.Domain.Common;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class VendorItem : AuditableEntity<Guid>
    {
        public Guid VendorId { get; private set; }
        public Guid ItemId { get; private set; }

        public Money UnitPrice { get; private set; }
        public int LeadTimeDays { get; private set; }
        public int MinOrderQty { get; private set; } = 1;

        public bool IsActive { get; private set; } = true;
        public DateTime EffectiveFromUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? EffectiveToUtc { get; private set; }

        public Vendor? Vendor { get; private set; }
        public Item? Item { get; private set; }

        private VendorItem() { }

        public VendorItem(Guid vendorId, Guid itemId, Money unitPrice, int leadTimeDays, int minOrderQty = 1,
                          DateTime? effectiveFromUtc = null, DateTime? effectiveToUtc = null)
        {
            Id = Guid.NewGuid();
            if (vendorId == Guid.Empty || itemId == Guid.Empty) throw new DomainException("Link ids required.");
            VendorId = vendorId; ItemId = itemId;
            SetCommercials(unitPrice, leadTimeDays, minOrderQty);
            SetEffectiveWindow(effectiveFromUtc ?? DateTime.UtcNow, effectiveToUtc);
        }

        public void SetCommercials(Money price, int lead, int moq)
        {
            if (price.Amount < 0) throw new DomainException("Unit price cannot be negative.");
            if (lead < 0) throw new DomainException("Lead time cannot be negative.");
            if (moq <= 0) throw new DomainException("MOQ must be positive.");
            UnitPrice = price; LeadTimeDays = lead; MinOrderQty = moq; ModifiedAtUtc = DateTime.UtcNow;
        }

        public void SetEffectiveWindow(DateTime fromUtc, DateTime? toUtc)
        {
            if (toUtc.HasValue && toUtc.Value <= fromUtc) throw new DomainException("Invalid effective window.");
            EffectiveFromUtc = fromUtc; EffectiveToUtc = toUtc; ModifiedAtUtc = DateTime.UtcNow;
        }

        public void Activate() { IsActive = true; ModifiedAtUtc = DateTime.UtcNow; }
        public void Deactivate() { IsActive = false; ModifiedAtUtc = DateTime.UtcNow; }
    }
}
