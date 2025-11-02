using GoProcure.Application.CQRS.PurchaseRequests.Commands.CreatePurchaseRequest;
using GoProcure.Application.CQRS.PurchaseRequests.Queries.GetPurchaseRequestById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoProcure.Api.Controllers
{
    [ApiController]
    [Route("api/v1/purchaserequests")]
    public class PurchaseRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PurchaseRequestsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreatePurchaseRequestCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetPurchaseRequestByIdQuery(id));
            return result is null ? NotFound() : Ok(result);
        }
    }
}
