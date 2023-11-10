using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Domain.Employees;
using Supermarket.Core.Domain.ProductCategories;
using Supermarket.Core.Domain.Products;
using Supermarket.Core.Domain.Regions;
using Supermarket.Core.Domain.Sales;
using Supermarket.Core.Domain.SellingProducts;
using Supermarket.Core.Domain.SoldProducts;
using Supermarket.Core.Domain.StoragePlaces;
using Supermarket.Core.Domain.StoredProducts;
using Supermarket.Core.Domain.Supermarkets;
using Supermarket.Core.UseCases.Common;
using Supermarket.Infrastructure.Database;
using Supermarket.Infrastructure.Employees;
using Supermarket.Infrastructure.Products;
using Supermarket.Infrastructure.StoragePlaces;
using Supermarket.Infrastructure.Regions;
using Supermarket.Infrastructure.Supermarkets;
using Supermarket.Infrastructure.Sales;
using Supermarket.Infrastructure.SellingProducts;
using Supermarket.Infrastructure.SoldProducts;
using Supermarket.Infrastructure.ProductCategories;
using Supermarket.Infrastructure.StoredProducts;
using Supermarket.Infrastructure.Suppliers;
using Supermarket.Core.Domain.Suppliers;

namespace Supermarket.Infrastructure;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        // add configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
        serviceCollection.AddSingleton<IConfiguration>(configuration);
        
        // add database connection driver
        serviceCollection.AddConfigurationSection<DatabaseOptions>();
        serviceCollection.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            return new OracleConnection(options.ConnectionString);
        });

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
        serviceCollection.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        serviceCollection.AddScoped<IStoragePlaceRepository, StoragePlaceRepository>();
        serviceCollection.AddScoped<ISupermarketRepository, SupermarketRepository>();
        serviceCollection.AddScoped<IRegionRepository, RegionRepository>();
        serviceCollection.AddScoped<ISaleRepository, SaleRepository>();
        serviceCollection.AddScoped<ISellingProductRepository, SellingProductRepository>();
        serviceCollection.AddScoped<IStoredProductRepository, StoredProductRepository>();
        serviceCollection.AddScoped<ISoldProductRepository, SoldProductRepository>();
        serviceCollection.AddScoped<ISupplierRepository, SupplierRepository>();
        
        return serviceCollection;
    }
}