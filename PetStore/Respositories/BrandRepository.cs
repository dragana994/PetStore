using Microsoft.EntityFrameworkCore;
using PetStore.Entities;
using PetStore.Queries;
using PetStore.Respositories.IRepository;
using System.Linq;

namespace PetStore.Respositories
{
	public class BrandRepository : IBrandRepository
	{
		private readonly PetStoreDbContext dbContext;

		public BrandRepository(PetStoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public IQueryable<BrandEntity> GetAll(QueryParameters queryParameters)
		{
			var query = dbContext.Brands
				.AsNoTracking()
				.AsQueryable();

			if (!string.IsNullOrEmpty(queryParameters.Search))
			{
				query = query
					.Where(x => EF.Functions.Like(x.Name, "%" + queryParameters.Search + "%"));
			}

			if (queryParameters.Paging)
			{
				query = query
				.Skip((queryParameters.Page - 1) * queryParameters.PageSize)
					.Take(queryParameters.PageSize);
			}

			return query;
		}

		public BrandEntity GetById(int brandId)
		{
			return dbContext.Brands
				.Where(x => x.BrandId == brandId)
				.FirstOrDefault();
		}

		public bool FindById(int brandId)
		{
			return dbContext.Brands
				.Where(x => x.BrandId == brandId)
				.Any();
		}
	}
}