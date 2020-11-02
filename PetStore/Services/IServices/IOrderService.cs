using PetStore.Models;
using PetStore.Queries;

namespace PetStore.Services.IServices
{
	public interface IOrderService
	{
		PagedList<OrderModel> GetAll(QueryParameters queryParameters);
		OrderModel GetById(int orderId);

		int Add(OrderModel order);
	}
}