using Mapster;
using PetStore.Entities;
using PetStore.Exceptions;
using PetStore.Extensions;
using PetStore.Models;
using PetStore.Queries;
using PetStore.Respositories.IRepository;
using PetStore.Services.IServices;
using PetStore.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository orderRepository;
		private readonly IOrderItemRepository orderItemRepository;
		private readonly IProductRepository productRepository;

		private readonly IValidator<OrderEntity> orderValidator;
		private readonly IValidator<OrderItemEntity> orderItemValidator;

		public OrderService(
			IOrderRepository orderRepository,
			IOrderItemRepository orderItemRepository,
			IProductRepository productRepository,
			IValidator<OrderEntity> orderValidator,
			IValidator<OrderItemEntity> orderItemValidator)
		{
			this.orderRepository = orderRepository;
			this.orderItemRepository = orderItemRepository;
			this.productRepository = productRepository;
			this.orderValidator = orderValidator;
			this.orderItemValidator = orderItemValidator;
		}

		public PagedList<OrderModel> GetAll(QueryParameters queryParameters)
		{
			var list = orderRepository
				.GetAll(queryParameters)
				.ProjectToType<OrderModel>()
				.ToList();

			var totalItem = orderRepository
				.GetTotalItemCount();

			return list.ToPagedList(queryParameters, totalItem);
		}

		public OrderModel GetById(int orderId)
		{
			var model = orderRepository
				.GetById(orderId)
				.Adapt<OrderModel>();

			if (model == null)
				return null;

			model.Items = orderItemRepository
				.GetByOrderId(orderId)
				.Adapt<List<OrderItemModel>>();

			model.Items.ForEach(x => model.Amount += (x.Amount * x.UnitPrice));

			return model;
		}

		public int Add(OrderModel model)
		{
			var newOrder = model.Adapt<OrderEntity>();

			newOrder.Date = DateTime.Now;
			newOrder.Status = OrderStatus.Successful;

			if (model.Items.Count <= 0)
				throw new ValidatorException("Can not create order without items");

			CreateOrderItem(newOrder, model);

			orderValidator.Validate(newOrder);

			orderRepository.Add(newOrder);

			return newOrder.OrderId;
		}

		#region Private methods

		private void CreateOrderItem(OrderEntity newOrder, OrderModel model)
		{
			var newOrderItems = new List<OrderItemEntity>();

			var groupOrderItemModels = model.Items
				.GroupBy(x => x.ProductId)
				.Select(gr => new OrderItemModel
				{
					ProductId = gr.Key,
					Amount = gr.Sum(x => x.Amount)
				});

			foreach (var orderItem in groupOrderItemModels)
			{
				var newOrderItem = orderItem.Adapt<OrderItemEntity>();
				newOrderItem.OrderId = newOrder.OrderId;
				newOrderItem.Order = newOrder;

				newOrderItem.Product = productRepository
					.GetById(orderItem.ProductId);
				if (newOrderItem.Product == null)
					throw new ValidatorException("Unkonwn product");

				newOrderItem.UnitPrice = newOrderItem.Product.Price;

				orderItemValidator.Validate(newOrderItem);

				newOrderItem.Product.Amount -= newOrderItem.Amount;

				newOrderItems.Add(newOrderItem);
			}

			orderItemRepository.AddBulk(newOrderItems);
		}

		#endregion
	}
}