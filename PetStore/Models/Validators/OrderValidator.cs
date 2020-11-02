using FluentValidation;
using System.Linq;

namespace PetStore.Models.Validators
{
	public class OrderValidator : AbstractValidator<OrderModel>
	{
		public OrderValidator()
		{
			RuleFor(p => p.CustomerFirstName)
				.NotEmpty().WithMessage("{PropertyName} is required");

			RuleFor(p => p.CustomerLastName)
				.NotEmpty().WithMessage("{PropertyName} is required");

			RuleFor(p => p.CustomerAddress)
				.NotEmpty().WithMessage("{PropertyName} is required");

			RuleFor(p => p.CustomerCreditCardNumber)
				.NotEmpty().WithMessage("{PropertyName} is required")
				.Must(ValidateCreditCard).WithMessage("{PropertyName} is not valid");
		}

		private bool ValidateCreditCard(string creditCard)
		{
			return creditCard.All(char.IsDigit);
		}
	}
}