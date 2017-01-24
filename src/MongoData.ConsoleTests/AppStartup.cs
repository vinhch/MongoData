using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System;
using MongoData.Repository;

namespace MongoData.ConsoleTests
{
    public static class AppStartup
    {
        public static IConfigurationRoot Configuration { get; private set; }
        public static IServiceProvider ServiceProvider { get; private set; }
        public static void Run()
        {
            Startup();
            ConfigureServices();
        }

        private static void Startup()
        {
            if (Configuration == null)
            {
                // get appsettings
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
            }
        }

        /// <summary>
        /// Config .Net Core built in Dependency Injection
        /// </summary>
        private static void ConfigureServices()
        {
            if (ServiceProvider != null) return;

            var services = new ServiceCollection()
                .AddScoped<IUnitOfWork>(provider => new UnitOfWork(Configuration.GetConnectionString("DefaultConnection")))
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<TestDb>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
