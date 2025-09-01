namespace ProductService.Application.Commands
{
    using MediatR;
    using ProductService.Application.Dtos;
    using ProductService.Domain.Entities;
    using ProductService.Domain.Repositories;

    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repo;
        private readonly IUnitOfWork _uow;

        public CreateProductHandler(IProductRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Product(request.Name, request.Description, request.Price, request.Stock);

            await _repo.AddAsync(entity, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return new ProductDto(entity.Id, entity.Name, entity.Description, entity.Price, entity.Stock, entity.CreatedAt, entity.UpdatedAt);
        }
    }
}
