namespace ProductService.Application.Commands
{
    using MediatR;
    using ProductService.Application.Dtos;
    using ProductService.Domain.Repositories;

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto?>
    {
        private readonly IProductRepository _repo;
        private readonly IUnitOfWork _uow;

        public UpdateProductHandler(IProductRepository repo, IUnitOfWork uow)
        {
            _repo = repo; 
            _uow = uow;
        }

        public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (product is null) 
                throw new KeyNotFoundException();

            product.Update(request.Name, request.Description, request.Price, request.Stock);
            await _repo.UpdateAsync(product, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.Stock, product.CreatedAt, product.UpdatedAt);
        }
    }
}
