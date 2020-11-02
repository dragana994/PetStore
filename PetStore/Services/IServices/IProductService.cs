using PetStore.Models;
using PetStore.Queries;

namespace PetStore.Services.IServices
{
	public interface IProductService
	{
		PagedList<ProductModel> GetAll(QueryParameters queryParameters);
		ProductModel GetById(int productId);

		int Add(ProductModel product);
		void Edit(ProductModel product);
		void Delete(int productId);
	}
}