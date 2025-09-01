namespace ProductService.Application.Queries
{
    using Common;
    using MediatR;
    using ProductService.Application.Dtos;
    using ProductService.Domain.Repositories;

    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var totalItems = await _repository.CountAsync();
            var products = await _repository.GetPaginateAsync(request.Filter);
            var productsDto =  products.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.CreatedAt,
                p.UpdatedAt
            ));

            return new PagedResult<ProductDto>
            {
                Items = productsDto,
                TotalItems = totalItems,
                PageNumber = request.Filter.PageNumber,
                PageSize = request.Filter.PageSize
            };
        }
    }
}
