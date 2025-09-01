namespace OrderService.Application.Validators
{
    using FluentValidation;
    using OrderService.Application.Commands;

    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.NewStatus).IsInEnum();
        }
    }
}
