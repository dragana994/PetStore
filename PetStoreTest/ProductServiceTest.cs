using Moq;
using PetStore.Entities;
using PetStore.Models;
using PetStore.Respositories.IRepository;
using PetStore.Services;
using PetStore.Services.IServices;
using PetStore.Validators;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PetStoreTest
{
	public class ProductServiceTest
	{
		private readonly IProductService service;

		public ProductServiceTest()
		{
			#region Mock BrandRepository

			var brandMockRepository = new Mock<IBrandRepository>();

			IList<BrandEntity> brands = new List<BrandEntity>
			{
				new BrandEntity
				{
					BrandId = 1,
					Name = "BrandTest1"
				},
				new BrandEntity
				{
					BrandId = 2,
					Name = "BrandTest2"
				}
			};

			brandMockRepository
				.Setup(repo => repo.FindById(It.IsAny<int>()))
				.Returns((int id) => brands.Where(x => x.BrandId == id).Any());

			brandMockRepository.SetupAllProperties();

			#endregion

			#region Mock CategoryRepository

			var categoryMockRepository = new Mock<ICategoryRepository>();

			IList<CategoryEntity> categories = new List<CategoryEntity>
			{
				new CategoryEntity
				{
					CategoryId = 1,
					Name = "CategoryTest1",
					Description = "CategoryTestDescription1"
				},
				new CategoryEntity
				{
					CategoryId = 2,
					Name = "CategoryTest2",
					Description = "CategoryTestDescription2"
				}
			};

			categoryMockRepository
				.Setup(repo => repo.FindByIds(It.IsAny<List<int>>()))
				.Returns((List<int> ids) => ids.All(x => categories.Select(x => x.CategoryId).ToList().Contains(x)));

			categoryMockRepository.SetupAllProperties();

			#endregion

			#region Mock ProductRepository

			var productMockReposotory = new Mock<IProductRepository>();

			IList<ProductEntity> products = new List<ProductEntity>
			{
				new ProductEntity
				{
					ProductId = 1,
					Name = "ProductTest1",
					Description = "ProductDescriptionTest1",
					Review = 1,
					Price = 1,
					Amount = 1,
					BrandId = 1
				},
				new ProductEntity
				{
					ProductId = 2,
					Name = "ProductTest2",
					Description = "ProductDescriptionTest2",
					Review = 2,
					Price = 2,
					Amount = 2,
					BrandId = 2
				},
			};

			productMockReposotory
				.Setup(repo => repo.GetById(It.IsAny<int>()))
				.Returns((int id) => products.Where(x => x.ProductId == id).FirstOrDefault());

			productMockReposotory
				.Setup(repo => repo.Add(It.IsAny<ProductEntity>()))
				.Returns((ProductEntity entity) =>
				{
					int maxId = products.Count();
					entity.ProductId = ++maxId;
					products.Add(entity);
					return maxId;
				});

			productMockReposotory
				.Setup(repo => repo.Edit(It.IsAny<ProductEntity>()))
				.Callback((ProductEntity entity) =>
				{
					products.Remove(products.ElementAt(products.IndexOf(products.Where(x => x.ProductId == entity.ProductId).FirstOrDefault())));
					products.Add(entity);
				}
			);

			productMockReposotory
				.Setup(repo => repo.Delete(It.IsAny<ProductEntity>()))
				.Callback((ProductEntity entity) => products.Remove(entity));

			productMockReposotory.SetupAllProperties();

			#endregion

			#region Mock ProductCategoryRepository

			var productCategoryMockRepository = new Mock<IProductCategoryRepository>();

			IList<ProductCategoryEntity> productCategories = new List<ProductCategoryEntity>
			{
				new ProductCategoryEntity
				{
					ProductId = 1,
					CategoryId = 1
				}
			};

			productCategoryMockRepository
				.Setup(repo => repo.GetByProductId(It.IsAny<int>()))
				.Returns((int productId) =>
				{
					var productCategory = productCategories
					.Where(x => x.ProductId == productId)
					.ToList();

					return productCategory.AsQueryable();
				});

			productCategoryMockRepository
				.Setup(repo => repo.AddCategories(It.IsAny<List<ProductCategoryEntity>>()))
				.Callback((List<ProductCategoryEntity> entities) =>
				{
					int maxId = products.Count();
					maxId++;
					entities.ForEach(e =>
					{
						e.ProductId = maxId;
						productCategories.Add(e);
					});
				}
			);

			productCategoryMockRepository
				.Setup(repo => repo.DeleteCategories(It.IsAny<List<ProductCategoryEntity>>()))
				.Callback((List<ProductCategoryEntity> entities) => entities.ForEach(e => productCategories.Remove(e)));

			productCategoryMockRepository.SetupAllProperties();

			#endregion

			#region Mock ProductValidator

			var productMockValidator = new Mock<IValidator<ProductEntity>>();

			productMockValidator.SetupAllProperties();

			#endregion

			service = new ProductService(
				productMockReposotory.Object,
				categoryMockRepository.Object,
				productCategoryMockRepository.Object,
				brandMockRepository.Object,
				productMockValidator.Object);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Add(bool setAllData)
		{
			var model = new ProductModel
			{
				Name = "TestProduct",
				Description = "TestDescription",
				Review = 2,
				Price = 5.5,
				Amount = 15,
				BrandId = setAllData ? 1 : (int?)null,
				Categories = new List<CategoryModel>()
			};

			if (setAllData)
			{
				model.Categories.Add(new CategoryModel
				{
					CategoryId = 1
				});
			}

			var id = service.Add(model);

			Assert.NotNull(service.GetById(id));
		}

		[Fact]
		public void Edit()
		{
			var product = service.GetById(1);

			if (product == null)
				return;

			product.Name = "TestProduct UPD";
			product.Description = "TestDescription UPD";

			product.Categories.Add(
				new CategoryModel
				{
					CategoryId = 2
				}
				);

			service.Edit(product);

			Assert.Equal(product.Name, service.GetById(1).Name);
		}

		[Fact]
		public void Delete()
		{
			service.Delete(1);

			Assert.Null(service.GetById(1));
		}
	}
}