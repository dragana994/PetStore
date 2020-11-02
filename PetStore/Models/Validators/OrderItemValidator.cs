using FluentValidation;

namespace PetStore.Models.Validators
{
	public class OrderItemValidator : AbstractValidator<OrderItemModel>
	{
		public OrderItemValidator()
		{
			RuleFor(p => p.ProductId)
				.NotEmpty().WithMessage("{PropertyName} is required");

			RuleFor(p => p.Amount)
				.NotEmpty().WithMessage("{PropertyName} is required")
				.GreaterThan(0).WithMessage("{PropertyName} must be greater then zero");
		}
	}
}