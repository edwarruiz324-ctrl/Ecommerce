namespace ProductService.Application.Queries
{
    using MediatR;
    using ProductService.Application.Dtos;

    public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;
}
