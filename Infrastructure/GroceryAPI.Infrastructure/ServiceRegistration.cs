using GroceryAPI.Application.Abstractions.Services;
using GroceryAPI.Application.Abstractions.Services.Configurations;
using GroceryAPI.Application.Abstractions.Storage;
using GroceryAPI.Application.Abstractions.Token;
using GroceryAPI.Infrastructure.Services;
using GroceryAPI.Infrastructure.Services.Configurations;
using GroceryAPI.Infrastructure.Services.Storage;
using GroceryAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            serviceCollection.AddScoped<IMailService, MailService>();
            serviceCollection.AddScoped<IApplicationService, ApplicationService>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }

    }
}
