namespace ProductService.Application.Queries
{
    using Common;
    using Common.Models;
    using MediatR;
    using ProductService.Application.Dtos;

    public record GetAllProductsQuery(PaginationFilter Filter) : IRequest<PagedResult<ProductDto>>;
}
