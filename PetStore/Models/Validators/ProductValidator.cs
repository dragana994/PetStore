using FluentValidation;

namespace PetStore.Models.Validators
{
	public class ProductValidator : AbstractValidator<ProductModel>
	{
		public ProductValidator()
		{
			RuleFor(p => p.Name)
			.NotEmpty().WithMessage("{PropertyName} is required")
			.MaximumLength(30).WithMessage("{PropertyName} can not have more then 30 characters");

			RuleFor(p => p.Price)
			.NotEmpty().WithMessage("{PropertyName} is required")
			.GreaterThan(0).WithMessage("{PropertyName} must be greate then zero");

			RuleFor(p => p.Amount)
			.NotEmpty().WithMessage("{PropertyName} is required")
			.GreaterThan(0).WithMessage("{PropertyName} must be greate then zero");
		}
	}
}