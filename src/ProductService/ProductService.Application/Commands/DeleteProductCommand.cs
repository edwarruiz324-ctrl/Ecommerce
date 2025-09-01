
namespace ProductService.Application.Commands
{
    using MediatR;
    public record DeleteProductCommand(int Id) : IRequest<bool>;
}
