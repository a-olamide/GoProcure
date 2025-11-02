using GoProcure.Application.Abstraction;
using GoProcure.Application.Abstractions.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.Behaviors
{
    public sealed class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(
            IUnitOfWork uow,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("----- Starting transaction for request {Request}", requestName);

            try
            {
                // Execute handler (and nested behaviors) first
                var response = await next();

                // Commit changes (EF SaveChanges or equivalent)
                await _uow.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("----- Transaction committed successfully for {Request}", requestName);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "----- Transaction rolled back for {Request}", requestName);
                throw;
            }
        }
    }
}
