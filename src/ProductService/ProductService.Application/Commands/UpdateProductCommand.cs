namespace ProductService.Application.Commands
{
    using MediatR;
    using ProductService.Application.Dtos;

    public record UpdateProductCommand(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int Stock
    ) : IRequest<ProductDto>;
}
