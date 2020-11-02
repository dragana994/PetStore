using Microsoft.EntityFrameworkCore;
using PetStore.Entities;
using PetStore.Queries;
using PetStore.Respositories.IRepository;
using System.Linq;

namespace PetStore.Respositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly PetStoreDbContext dbContext;

		public OrderRepository(PetStoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public IQueryable<OrderEntity> GetAll(QueryParameters queryParameters)
		{
			var query = dbContext.Orders
				.AsNoTracking()
				.AsQueryable();

			if (!string.IsNullOrEmpty(queryParameters.Search))
			{
				query = query
					.Where(x => EF.Functions.Like(x.CustomerCreditCardNumber, "%" + queryParameters.Search + "%"));
			}

			if (queryParameters.Paging)
			{
				query = query
				.Skip((queryParameters.Page - 1) * queryParameters.PageSize)
					.Take(queryParameters.PageSize);
			}

			return query;
		}

		public OrderEntity GetById(int orderId)
		{
			return dbContext.Orders
				.Where(x => x.OrderId == orderId)
				.FirstOrDefault();
		}

		public int GetTotalItemCount()
		{
			return dbContext.Orders
				.Count();
		}

		public int Add(OrderEntity orderEntity)
		{
			dbContext.Add(orderEntity);

			dbContext.SaveChanges();

			return orderEntity.OrderId;
		}
	}
}