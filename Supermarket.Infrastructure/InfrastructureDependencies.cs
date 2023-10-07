﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Supermarket.Core.Common;
using Supermarket.Domain.Employees;
using Supermarket.Domain.Products;
using Supermarket.Domain.Products.Categories;
using Supermarket.Infrastructure.Common;
using Supermarket.Infrastructure.Database;
using Supermarket.Infrastructure.Employees;
using Supermarket.Infrastructure.Products;
using Supermarket.Infrastructure.Products.Categories;

namespace Supermarket.Infrastructure;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        // add configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        serviceCollection.AddSingleton<IConfiguration>(configuration);
        
        // add database connection driver
        serviceCollection.AddConfigurationSection<DatabaseOptions>();
        serviceCollection.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            return ActivatorUtilities.CreateInstance<OracleConnection>(sp, options.ConnectionString);
        });

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
        serviceCollection.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        
        return serviceCollection;
    }
}