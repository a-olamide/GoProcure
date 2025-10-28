using GoProcure.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Domain.ValueObjects
{
    public sealed class BankAccountVO
    {
        public string BankName { get; private set; } = default!;
        public string AccountName { get; private set; } = default!;
        public string AccountNumber { get; private set; } = default!;

        private BankAccountVO() { } // EF

        public BankAccountVO(string bank, string name, string number)
        {
            if (string.IsNullOrWhiteSpace(bank)) throw new DomainException("BankName required.");
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("AccountName required.");
            if (string.IsNullOrWhiteSpace(number)) throw new DomainException("AccountNumber required.");
            BankName = bank.Trim();
            AccountName = name.Trim();
            AccountNumber = number.Trim();
        }
    }
}
