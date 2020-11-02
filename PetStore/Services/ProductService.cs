using Mapster;
using PetStore.Entities;
using PetStore.Exceptions;
using PetStore.Extensions;
using PetStore.Models;
using PetStore.Queries;
using PetStore.Respositories.IRepository;
using PetStore.Services.IServices;
using PetStore.Validators;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository productRepository;
		private readonly ICategoryRepository categoryRepository;
		private readonly IProductCategoryRepository productCategoryRepository;
		private readonly IBrandRepository brandRepository;

		private readonly IValidator<ProductEntity> productValidator;

		public ProductService(
			IProductRepository productRepository,
			ICategoryRepository categoryRepository,
			IProductCategoryRepository productCategoryRepository,
			IBrandRepository brandRepository,
			IValidator<ProductEntity> productValidator)
		{
			this.productRepository = productRepository;
			this.categoryRepository = categoryRepository;
			this.productCategoryRepository = productCategoryRepository;
			this.brandRepository = brandRepository;
			this.productValidator = productValidator;
		}

		public PagedList<ProductModel> GetAll(QueryParameters queryParameters)
		{
			var list = productRepository
				.GetAll(queryParameters)
				.ProjectToType<ProductModel>()
				.ToList();

			var totalItem = productRepository
				.GetTotalItemCount();

			return list.ToPagedList(queryParameters, totalItem);
		}

		public ProductModel GetById(int productId)
		{
			var model = productRepository
				.GetById(productId)
				.Adapt<ProductModel>();

			if (model == null)
				return null;

			model.Categories = productCategoryRepository
				.GetByProductId(productId)
				.Adapt<List<CategoryModel>>();

			return model;
		}

		public int Add(ProductModel model)
		{
			var newProduct = model.Adapt<ProductEntity>();

			if (model.BrandId.HasValue)
			{
				if (!brandRepository.FindById(model.BrandId.Value))
					throw new ValidatorException("Unkonwn brand");
			}

			if (model.Categories.Count > 0)
			{
				var categoryIds = model.Categories
					.Select(x => x.CategoryId)
					.ToList();

				if (!categoryRepository.FindByIds(categoryIds))
					throw new ValidatorException("Unkonwn category");

				var newProductCategories = new List<ProductCategoryEntity>();

				foreach (var category in model.Categories)
				{
					var newProductCategory = new ProductCategoryEntity
					{
						ProductId = newProduct.ProductId,
						Product = newProduct,
						CategoryId = category.CategoryId
					};

					newProductCategories.Add(newProductCategory);
				}

				productCategoryRepository.AddCategories(newProductCategories);
			}

			productValidator.Validate(newProduct);

			productRepository.Add(newProduct);

			return newProduct.ProductId;
		}

		public void Edit(ProductModel model)
		{
			var dbProduct = productRepository
				.GetById(model.ProductId.Value);

			if (dbProduct == null)
				throw new ValidatorException("Unkonwn product");

			dbProduct.Name = model.Name;
			dbProduct.Description = model.Description;
			dbProduct.Review = model.Review;

			dbProduct.Price = model.Price;
			dbProduct.Amount = model.Amount;

			dbProduct.ImagePath = model.ImagePath;

			if (model.BrandId != dbProduct.BrandId)
			{
				if (model.BrandId.HasValue && !brandRepository.FindById(model.BrandId.Value))
					throw new ValidatorException("Unkonwn brand");

				dbProduct.BrandId = model.BrandId;
			}

			UpdateProductCategories(model);

			productValidator.Validate(dbProduct);

			productRepository.Edit(dbProduct);
		}

		public void Delete(int productId)
		{
			var dbProduct = productRepository
				.GetById(productId);

			if (dbProduct == null)
				throw new ValidatorException("Unkonwn product");

			var dbProductCategories = productCategoryRepository
				.GetByProductId(productId)
				.ToList();

			productCategoryRepository.DeleteCategories(dbProductCategories);

			productRepository.Delete(dbProduct);
		}

		#region Private methods

		private void UpdateProductCategories(ProductModel model)
		{
			var dbProductCategoryIds = productCategoryRepository
				.GetByProductId(model.ProductId.Value)
				.Select(x => x.CategoryId)
				.ToList();

			var modelProductCategoryIds = model.Categories
				.Select(x => x.CategoryId)
				.ToList();

			var newIds = modelProductCategoryIds
				.Except(dbProductCategoryIds)
				.ToList();

			if (!categoryRepository.FindByIds(newIds))
				throw new ValidatorException("Unkown category");

			var newCategories = model.Categories
				.Where(x => newIds.Contains(x.CategoryId))
				.Adapt<List<ProductCategoryEntity>>();
			newCategories.ForEach(x => x.ProductId = model.ProductId.Value);

			productCategoryRepository.AddCategories(newCategories);

			var removeIds = dbProductCategoryIds
				.Except(modelProductCategoryIds)
				.ToList();

			var removeCategories = model.Categories
				.Where(x => removeIds.Contains(x.CategoryId))
				.Adapt<List<ProductCategoryEntity>>();
			productCategoryRepository.DeleteCategories(removeCategories);
		}

		#endregion
	}
}