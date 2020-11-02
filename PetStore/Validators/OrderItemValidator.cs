using PetStore.Entities;
using PetStore.Exceptions;

namespace PetStore.Validators
{
	public class OrderItemValidator : BaseValidator<OrderItemEntity>
	{
		protected override void ExtendedValidation(OrderItemEntity entity)
		{
			if (entity.Amount > entity.Product.Amount)
				throw new ValidatorException("The number of order items must be less than the number of available items");
		}
	}
}