using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MongoData.Tests
{
    public static class Utils
    {
        private static IConfigurationRoot _configuration;
        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }

        private static IUnitOfWork _mongoUnitOfWork;
        public static IUnitOfWork MongoUnitOfWork
        {
            get
            {
                if (_mongoUnitOfWork == null)
                {
                    _mongoUnitOfWork = new UnitOfWork(Configuration.GetConnectionString("DefaultConnection"));
                }
                return _mongoUnitOfWork;
            }
        }

        public static void DropDb()
        {
            MongoUnitOfWork.Client.DropDatabase(MongoUnitOfWork.Database.DatabaseNamespace.DatabaseName);
        }
    }
}
