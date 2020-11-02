using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetStore.Entities;
using PetStore.Exceptions;
using PetStore.RequestLogger;
using PetStore.Respositories;
using PetStore.Respositories.IRepository;
using PetStore.Services;
using PetStore.Services.IServices;
using PetStore.Settings;
using PetStore.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddControllers()
				.AddFluentValidation(s =>
				{
					s.RegisterValidatorsFromAssemblyContaining<Startup>();
					s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
				});

			//Database
			services.AddEntityFrameworkSqlite()
			 .AddDbContext<PetStoreDbContext>();

			services.AddScoped<PetStoreDbContext, PetStoreDbContext>();

			services.AddIdentity<UserEntity, RoleEntity>()
				.AddEntityFrameworkStores<PetStoreDbContext>()
				.AddDefaultTokenProviders();

			//JWT Token
			services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
			var jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>();
			services
							 .AddAuthorization()
							 .AddAuthentication(options =>
							 {
								 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
								 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
								 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
							 })
							 .AddJwtBearer(options =>
							 {
								 options.RequireHttpsMetadata = false;
								 options.TokenValidationParameters = new TokenValidationParameters
								 {
									 ValidIssuer = jwtSettings.Issuer,
									 ValidAudience = jwtSettings.Issuer,
									 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
									 ClockSkew = TimeSpan.Zero
								 };
							 });

			//Repositories
			services.AddScoped<IBrandRepository, BrandRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IOrderItemRepository, OrderItemRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

			//Services
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IProductService, ProductService>();

			//Validators
			services.AddScoped<IValidator<ProductEntity>, BaseValidator<ProductEntity>>();
			services.AddScoped<IValidator<OrderEntity>, BaseValidator<OrderEntity>>();
			services.AddScoped<IValidator<OrderItemEntity>, OrderItemValidator>();

			//ExceptionFilter
			services.AddMvc(options =>
			{
				options.Filters.Add(typeof(ExceptionFilter));
			});

			//Swagger
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pet Store", Version = "v1" });

				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT containing userid claim",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
				});
				var security = new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer",
								Type = ReferenceType.SecurityScheme
							},
							UnresolvedReference = true
						},
						new List<string>()
					}
				};
				options.AddSecurityRequirement(security);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			//RequestLogger
			app.UseMiddleware<RequestLoggingMiddleware>();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			//Swagger
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.RoutePrefix = "";
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet Store V1");
			});
		}
	}
}