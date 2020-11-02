using PetStore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Respositories.IRepository
{
	public interface IOrderItemRepository
	{
		IQueryable<OrderItemEntity> GetByOrderId(int orderId);

		short Add(OrderItemEntity orderItemEntity);
		List<short> AddBulk(List<OrderItemEntity> list);
	}
}