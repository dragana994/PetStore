using Microsoft.EntityFrameworkCore;
using PetStore.Entities;
using PetStore.Respositories.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Respositories
{
	public class OrderItemRepository : IOrderItemRepository
	{
		private readonly PetStoreDbContext dbContext;

		public OrderItemRepository(PetStoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public IQueryable<OrderItemEntity> GetByOrderId(int orderId)
		{
			return dbContext.OrderItems
				.AsNoTracking()
				.Include(x => x.Order)
				.Include(x => x.Product)
				.Where(x => x.OrderId == orderId);
		}

		public short Add(OrderItemEntity orderItemEntity)
		{
			dbContext.Add(orderItemEntity);

			return orderItemEntity.OrderItemId;
		}

		public List<short> AddBulk(List<OrderItemEntity> list)
		{
			dbContext.AddRange(list);

			return list.Select(x => x.OrderItemId).ToList();
		}
	}
}