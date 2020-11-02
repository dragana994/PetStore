using PetStore.Exceptions;
using PetStore.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Validators
{
	public class BaseValidator<TEntity> : IValidator<TEntity>
	{
		public void Validate(TEntity entity)
		{
			if (entity == null)
				throw new ValidatorException("Entity can not be null");

			var context = new ValidationContext(entity, serviceProvider: null, items: null);
			var results = new List<ValidationResult>();

			var isValid = Validator.TryValidateObject(entity, context, results);

			if (!isValid)
				throw new ValidatorException(results.ToErrorMessageList());

			ExtendedValidation(entity);
		}

		protected virtual void ExtendedValidation(TEntity entity) { }
	}
}