using GoProcure.Domain.Common;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.Entities
{
    public sealed class Vendor : AuditableEntity<Guid>
    {
        public string Name { get; private set; } = default!;
        public string TaxId { get; private set; } = default!;
        public Email Email { get; private set; } = Email.Create("noreply@example.com");
        public PhoneNumber Phone { get; private set; } = PhoneNumber.Create("+10000000000"); // placeholder default; will be set by ctor
        public BankAccountVO Bank { get; private set; } = default!;
        public bool IsActive { get; private set; } = true;

        private Vendor() { }

        public Vendor(string name, string taxId, Email email, PhoneNumber phone, BankAccountVO bank)
        {
            Id = Guid.NewGuid();
            SetName(name);
            SetTaxId(taxId);
            Email = email;
            Phone = phone;
            Bank = bank;
        }

        public void UpdateContact(Email email, PhoneNumber phone)
        {
            Email = email;
            Phone = phone;
            ModifiedAtUtc = DateTime.UtcNow;
        }

        public void UpdateProfile(string name, string taxId)
        {
            SetName(name);
            SetTaxId(taxId);
            ModifiedAtUtc = DateTime.UtcNow;
        }

        public void Activate() => IsActive = true;
        public void Suspend() => IsActive = false;

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 200)
                throw new DomainException("Invalid vendor name.");
            Name = name.Trim();
        }

        private void SetTaxId(string taxId)
        {
            if (string.IsNullOrWhiteSpace(taxId) || taxId.Length > 50)
                throw new DomainException("Invalid tax id.");
            TaxId = taxId.Trim();
        }
    }
}
