using Microsoft.EntityFrameworkCore;
using PetStore.Entities;
using PetStore.Respositories.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Respositories
{
	public class ProductCategoryRepository : IProductCategoryRepository
	{
		private readonly PetStoreDbContext dbContext;

		public ProductCategoryRepository(PetStoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public IQueryable<ProductCategoryEntity> GetByProductId(int productId)
		{
			return dbContext.ProductCategories
				.Include(x => x.Category)
				.Where(x => x.ProductId == productId);
		}

		public void AddCategories(List<ProductCategoryEntity> productCategoryEntities)
		{
			dbContext.AddRange(productCategoryEntities);
		}

		public void DeleteCategories(List<ProductCategoryEntity> productCategoryEntities)
		{
			dbContext.RemoveRange(productCategoryEntities);
		}
	}
}