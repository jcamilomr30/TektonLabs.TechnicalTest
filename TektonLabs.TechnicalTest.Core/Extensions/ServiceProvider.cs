using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TektonLabs.TechnicalTest.Core.Dtos;
using TektonLabs.TechnicalTest.Core.Interfaces;
using TektonLabs.TechnicalTest.Core.Services;
using TektonLabs.TechnicalTest.Domain;
using TektonLabs.TechnicalTest.Domain.IRepository;
using TektonLabs.TechnicalTest.Domain.Repository;
using System.Reflection;
using TektonLabs.TechnicalTest.Core.Cache;

namespace TektonLabs.TechnicalTest.Core.Extensions
{
    public static class ServiceProvider
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Repositories
            services.AddScoped<IQueryableUnitOfWork, ProjectNameContext>();
            services.AddScoped<IProductRepository, ProductRepository>();

            //Services
            services.AddScoped<IProductService, ProductService>();
            services.AddSingleton<ICacheProvider, CacheProvider>();
            services.AddScoped<IValidator<CreateProductDto>, CreateProductDtoValidator>();
            services.AddScoped<IValidator<UpdateProductDto>, UpdateProductDtoValidator>();

            services.AddAutoMapper(Assembly.Load("TektonLabs.TechnicalTest.Core"));
            services.AddDbContext<ProjectNameContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("database"), sqlopts =>
                {
                    sqlopts.MigrationsHistoryTable("_MigrationHistory", configuration.GetValue<string>("SchemaName"));
                });
            }, ServiceLifetime.Transient);
        }
    }
}
