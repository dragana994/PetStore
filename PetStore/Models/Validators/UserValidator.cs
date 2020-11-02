using FluentValidation;

namespace PetStore.Models.Validators
{
	public class UserValidator : AbstractValidator<UserModel>
	{
		public UserValidator()
		{
			RuleFor(p => p.FirstName)
			.NotEmpty().WithMessage("{PropertyName} is required")
			.MaximumLength(30).WithMessage("{PropertyName} can not have more then 30 characters");

			RuleFor(p => p.LastName)
			.NotEmpty().WithMessage("{PropertyName} is required")
			.MaximumLength(30).WithMessage("{PropertyName} can not have more then 30 characters");


			RuleFor(p => p.UserName)
			.NotEmpty().WithMessage("{PropertyName} is required");

			RuleFor(p => p.Password)
			.NotEmpty().WithMessage("{PropertyName} is required");

			RuleFor(p => p.Email)
			.NotEmpty().WithMessage("{PropertyName} is required");
		}
	}
}