using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Commands.CreatePurchaseRequest
{
    public sealed class CreatePurchaseRequestValidator : AbstractValidator<CreatePurchaseRequestCommand>
    {
        public CreatePurchaseRequestValidator()
        {
            //RuleFor(x => x.RequesterId).NotEmpty();
            RuleFor(x => x.Department).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Currency).NotEmpty().Length(3);
        }
    }
}
