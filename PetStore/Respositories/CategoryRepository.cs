using Microsoft.EntityFrameworkCore;
using PetStore.Entities;
using PetStore.Queries;
using PetStore.Respositories.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Respositories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly PetStoreDbContext dbContext;

		public CategoryRepository(PetStoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public IQueryable<CategoryEntity> GetAll(QueryParameters queryParameters)
		{
			var query = dbContext.Categories
				.AsNoTracking()
				.AsQueryable();

			if (!string.IsNullOrEmpty(queryParameters.Search))
			{
				query = query = query
					.Where(x => EF.Functions.Like(x.Name, "%" + queryParameters.Search + "%"));
			}

			if (queryParameters.Paging)
			{
				query
					.Skip((queryParameters.Page - 1) * queryParameters.PageSize)
					.Take(queryParameters.PageSize);
			}

			return query;
		}

		public CategoryEntity GetById(int categoryId)
		{
			return dbContext.Categories
				.Where(x => x.CategoryId == categoryId)
				.FirstOrDefault();
		}

		public bool FindByIds(List<int> categoryIds)
		{
			var dbCategoryIds = dbContext.Categories
				.Select(x => x.CategoryId)
				.ToList();

			return categoryIds
				.All(x => dbCategoryIds.Contains(x));
		}
	}
}