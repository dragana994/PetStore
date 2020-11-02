using PetStore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Respositories.IRepository
{
	public interface IProductCategoryRepository
	{
		IQueryable<ProductCategoryEntity> GetByProductId(int productId);

		void AddCategories(List<ProductCategoryEntity> productCategoryEntities);
		void DeleteCategories(List<ProductCategoryEntity> productCategoryEntities);
	}
}