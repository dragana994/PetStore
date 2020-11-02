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
	public class ProductController : ControllerBase
	{
		[HttpGet]
		[Produces(typeof(PagedList<ProductModel>))]
		public IActionResult GetAll([FromQuery]QueryParameters queryParameters, [FromServices] IProductService service)
		{
			return Ok(service.GetAll(queryParameters));
		}

		// GET: api/Product/5
		[HttpGet("{id}")]
		[Produces(typeof(ProductModel))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult Get(int id, [FromServices] IProductService service)
		{
			var model = service.GetById(id);

			return model == null ? NotFound() : (IActionResult)Ok(model);
		}

		[Authorize]
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
		public void Add([FromBody] ProductModel model, [FromServices] IProductService service)
		{
			model.ProductId = null;

			service.Add(model);
		}

		[Authorize]
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
		public void Edit(int id, [FromBody] ProductModel model, [FromServices] IProductService service)
		{
			model.ProductId = id;
			service.Edit(model);
		}

		[Authorize]
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(List<ErrorMessage>), StatusCodes.Status400BadRequest)]
		public void Delete(int id, [FromServices] IProductService service)
		{
			service.Delete(id);
		}
	}
}