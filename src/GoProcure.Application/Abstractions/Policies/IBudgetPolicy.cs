using GoProcure.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Abstraction.Policies
{
    public interface IBudgetPolicy
    {
        Money GetBudgetLimitFor(string department);
        bool CanSpend(string department, Money amount);
    }
}
