using Microsoft.EntityFrameworkCore;
using PetStore.Entities;
using PetStore.Queries;
using PetStore.Respositories.IRepository;
using System.Linq;

namespace PetStore.Respositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly PetStoreDbContext dbContext;

		public ProductRepository(PetStoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public IQueryable<ProductEntity> GetAll(QueryParameters queryParameters)
		{
			var query = dbContext.Products
				.AsNoTracking()
				.Include(x => x.Brand)
				.AsQueryable();

			if (!string.IsNullOrEmpty(queryParameters.Search))
			{
				query = query
					.Where(x => EF.Functions.Like(x.Name, "%" + queryParameters.Search + "%") ||
					 EF.Functions.Like(x.Brand.Name, "%" + queryParameters.Search + "%"));
			}

			if (queryParameters.Paging)
			{
				query = query
					.Skip((queryParameters.Page - 1) * queryParameters.PageSize)
					.Take(queryParameters.PageSize);
			}

			return query;
		}

		public int GetTotalItemCount()
		{
			return dbContext.Products
				.Count();
		}

		public bool FindById(int productId)
		{
			return dbContext.Products
				.Where(x => x.ProductId == productId)
				.Any();
		}

		public ProductEntity GetById(int productId)
		{
			return dbContext.Products
				.Include(x => x.Brand)
				.Where(x => x.ProductId == productId)
				.FirstOrDefault();
		}

		public int Add(ProductEntity productEntity)
		{
			dbContext.Add(productEntity);

			dbContext.SaveChanges();

			return productEntity.ProductId;
		}

		public void Edit(ProductEntity productEntity)
		{
			dbContext.Update(productEntity);

			dbContext.SaveChanges();
		}

		public void Delete(ProductEntity productEntity)
		{
			dbContext.Remove(productEntity);

			dbContext.SaveChanges();
		}
	}
}