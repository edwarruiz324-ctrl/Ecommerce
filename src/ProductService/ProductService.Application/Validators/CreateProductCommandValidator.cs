namespace ProductService.Application.Validators
{
    using FluentValidation;
    using ProductService.Application.Commands;

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(200).WithMessage("El nombre puede tener máximo 200 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción puede tener máximo 1000 caracteres.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("El precio debe ser mayor o igual a 0.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock debe ser mayor o igual a 0.");
        }
    }
}
