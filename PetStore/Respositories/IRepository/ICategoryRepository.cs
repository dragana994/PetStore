using PetStore.Entities;
using PetStore.Queries;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Respositories.IRepository
{
	public interface ICategoryRepository
	{
		IQueryable<CategoryEntity> GetAll(QueryParameters queryParameters);
		CategoryEntity GetById(int categoryId);

		bool FindByIds(List<int> categoryIds);
	}
}