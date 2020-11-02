using PetStore.Entities;
using PetStore.Queries;
using System.Linq;

namespace PetStore.Respositories.IRepository
{
	public interface IProductRepository
	{
		IQueryable<ProductEntity> GetAll(QueryParameters queryParameters);
		ProductEntity GetById(int productId);

		int GetTotalItemCount();
		bool FindById(int productId);

		int Add(ProductEntity productEntity);
		void Edit(ProductEntity productEntity);
		void Delete(ProductEntity productEntity);
	}
}