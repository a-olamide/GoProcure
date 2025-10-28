using GoProcure.Domain.Common;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class Item : AuditableEntity<Guid>
    {
        public string Sku { get; private set; } = default!;
        public string Name { get; private set; } = default!;
        public string Unit { get; private set; } = "EA";          // EA, BOX, KG, HOUR...
        public Money DefaultPrice { get; private set; } = Money.Zero();

        public Guid? PreferredVendorId { get; private set; }
        public Vendor? PreferredVendor { get; private set; }

        private Item() { }

        public Item(string sku, string name, string unit, Money defaultPrice, Guid? preferredVendorId = null)
        {
            Id = Guid.NewGuid();
            SetSku(sku);
            SetName(name);
            SetUnit(unit);
            SetDefaultPrice(defaultPrice);
            PreferredVendorId = preferredVendorId;
        }

        public void Rename(string name) { SetName(name); ModifiedAtUtc = DateTime.UtcNow; }
        public void ChangeUnit(string unit) { SetUnit(unit); ModifiedAtUtc = DateTime.UtcNow; }
        public void ChangeDefaultPrice(Money price) { SetDefaultPrice(price); ModifiedAtUtc = DateTime.UtcNow; }
        public void SetPreferredVendor(Guid? vendorId) { PreferredVendorId = vendorId; ModifiedAtUtc = DateTime.UtcNow; }

        private void SetSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku) || sku.Length > 100)
                throw new DomainException("Invalid SKU.");
            Sku = sku.Trim().ToUpperInvariant();
        }
        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 200)
                throw new DomainException("Invalid item name.");
            Name = name.Trim();
        }
        private void SetUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit) || unit.Length > 10)
                throw new DomainException("Invalid unit.");
            Unit = unit.Trim().ToUpperInvariant();
        }
        private void SetDefaultPrice(Money price)
        {
            if (price.Amount < 0 || string.IsNullOrWhiteSpace(price.Currency))
                throw new DomainException("Invalid default price.");
            DefaultPrice = price;
        }
    }
}
