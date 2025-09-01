namespace ProductService.Application.Commands
{
    using ProductService.Application.Dtos;
    using MediatR;

    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int Stock
    ) : IRequest<ProductDto>;
}
