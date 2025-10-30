using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            if (_validators.Any())
            {
                var ctx = new ValidationContext<TRequest>(request);
                var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(ctx, ct))))
                    .SelectMany(r => r.Errors)
                    .Where(f => f is not null)
                    .ToArray();

                if (failures.Length > 0)
                    throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
