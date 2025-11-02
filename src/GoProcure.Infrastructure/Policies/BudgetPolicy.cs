using GoProcure.Application.Abstraction.Policies;
using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure.Policies
{
    public sealed class BudgetPolicy : IBudgetPolicy
    {
        // Simple in-memory defaults; replace with DB lookups later.
        private static readonly Dictionary<string, Money> DeptLimits = new(StringComparer.OrdinalIgnoreCase)
        {
            ["IT"] = new Money(50_000m, "USD"),
            ["PROCUREMENT"] = new Money(100_000m, "USD"),
            ["FINANCE"] = new Money(75_000m, "USD"),
        };

        public Money GetBudgetLimitFor(string department)
            => DeptLimits.TryGetValue(department ?? "", out var limit) ? limit : Money.Zero("USD");

        public bool CanSpend(string department, Money amount)
            => amount.Amount <= GetBudgetLimitFor(department).Amount;
    }
}
