namespace OrderService.Api.Controllers
{
    using Common.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using OrderService.Application.Commands;
    using OrderService.Application.Dtos;
    using OrderService.Application.Queries;

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var command = new CreateOrderCommand(
            request.CustomerId,
            request.Items.Select(i => new CreateOrderItemDto(i.ProductId, i.Quantity, 0)).ToList());

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            if (id != request.OrderId)
                return BadRequest(new { Message = ErrorMessages.IdNotMatch });

            var command = new UpdateOrderStatusCommand(request.OrderId, request.NewStatus);

            var updated = await _mediator.Send(command);
            
            return Ok(updated);
        }
    }
}