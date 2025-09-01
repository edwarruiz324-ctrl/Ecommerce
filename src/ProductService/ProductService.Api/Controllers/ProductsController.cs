namespace ProductsService.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using ProductService.Application.Commands;
    using ProductService.Application.Dtos;
    using ProductService.Application.Queries;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request)
        {
            var command = new CreateProductCommand(request.Name, request.Description, request.Price, request.Stock);

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:int}", Name = nameof(GetById))]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product is null) return NotFound();
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductRequest request)
        {
            if (id != request.Id)
                return BadRequest(new { Message = "Los Identificadores no coinciden." });

            try
            {
                var command = new UpdateProductCommand(request.Id, request.Name, request.Description, request.Price, request.Stock);
                var updated = await _mediator.Send(command);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _mediator.Send(new DeleteProductCommand(id));
            return deleted ? NoContent() : NotFound();
        }
    }    
}
