namespace OrderService.Application.Validators
{
    using FluentValidation;
    using OrderService.Application.Commands;

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("El cliente es obligatorio");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("El pedido debe contener al menos un producto");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.ProductId)
                    .GreaterThan(0)
                    .WithMessage("El ProductId debe ser mayor que 0.");

                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("La cantidad debe ser mayor que 0.");

                items.RuleFor(i => i.UnitPrice)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("El precio unitario no puede ser negativo.");
            });
        }
    }
}
