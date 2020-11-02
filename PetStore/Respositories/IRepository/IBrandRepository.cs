using PetStore.Entities;
using PetStore.Queries;
using System.Linq;

namespace PetStore.Respositories.IRepository
{
	public interface IBrandRepository
	{
		IQueryable<BrandEntity> GetAll(QueryParameters queryParameters);
		BrandEntity GetById(int brandId);

		bool FindById(int brandId);
	}
}