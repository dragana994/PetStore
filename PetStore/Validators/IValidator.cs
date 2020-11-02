namespace PetStore.Validators
{
	public interface IValidator<TEntity>
	{
		void Validate(TEntity entity);
	}
}