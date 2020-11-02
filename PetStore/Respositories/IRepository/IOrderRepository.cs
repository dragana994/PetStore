using PetStore.Entities;
using PetStore.Queries;
using System.Linq;

namespace PetStore.Respositories.IRepository
{
	public interface IOrderRepository
	{
		IQueryable<OrderEntity> GetAll(QueryParameters queryParameters);
		OrderEntity GetById(int orderId);

		int GetTotalItemCount();

		int Add(OrderEntity orderEntity);
	}
}