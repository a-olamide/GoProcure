using AutoMapper;
using GoProcure.Application.Abstraction;
using GoProcure.Application.Abstraction.Policies;
using GoProcure.Application.Abstractions.Persistence;
using GoProcure.Application.Abstractions.Repositories;
using GoProcure.Application.CQRS.PurchaseRequests.Dtos;
using GoProcure.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Application.CQRS.PurchaseRequests.Commands.CreatePurchaseRequest
{
    public sealed class CreatePurchaseRequestHandler
        : IRequestHandler<CreatePurchaseRequestCommand, PurchaseRequestDto>
    {
        private readonly IPurchaseRequestRepository _repo;
        private readonly IBudgetPolicy _budget;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreatePurchaseRequestHandler(
            IPurchaseRequestRepository repo,
            IBudgetPolicy budget,
            IUnitOfWork uow,
            IMapper mapper)
        { _repo = repo; _budget = budget; _uow = uow; _mapper = mapper; }

        public async Task<PurchaseRequestDto> Handle(CreatePurchaseRequestCommand request, CancellationToken ct)
        {
            var limit = _budget.GetBudgetLimitFor(request.Department);
            var pr = new PurchaseRequest(request.RequesterId, request.Department,
                                         budgetLimit: limit.Amount > 0 ? limit : null);

            await _repo.AddAsync(pr, ct);
            await _uow.SaveChangesAsync(ct);

            return _mapper.Map<PurchaseRequestDto>(pr);
        }
    }
}
