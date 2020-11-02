using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetStore.Exceptions;
using PetStore.Models;
using PetStore.Queries;
using PetStore.Services.IServices;
using System.Collections.Generic;

namespace PetStore.ApiController
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		[Authorize]
		[HttpGet]
		[Produces(typeof(PagedList<OrderModel>))]
		public IActionResult Get([FromQuery]QueryParameters queryParameters, [FromServices] IOrderService service)
		{
			return Ok(service.GetAll(queryParameters));
		}

		[HttpGet("{id}")]
		[Authorize]
		[Produces(typeof(OrderModel))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(int id, [FromServices] IOrderService service)
		{
			var model = service.GetById(id);

			return model == null ? NotFound() : (IActionResult)Ok(model);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
		public void Post([FromBody] OrderModel model, [FromServices] IOrderService service)
		{
			model.OrderId = null;

			service.Add(model);
		}
	}
}