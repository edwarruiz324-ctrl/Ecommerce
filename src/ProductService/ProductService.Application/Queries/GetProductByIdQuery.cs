namespace ProductService.Application.Queries
{
    using MediatR;
    using ProductService.Application.Dtos;

    public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;
}
