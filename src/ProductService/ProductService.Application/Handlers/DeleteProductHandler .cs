namespace ProductService.Application.Commands
{
    using MediatR;
    using ProductService.Domain.Repositories;

    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _repo;
        private readonly IUnitOfWork _uow;

        public DeleteProductHandler(IProductRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (product == null) return false;

            await _repo.DeleteAsync(product, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
