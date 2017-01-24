using System;
using MongoDB.Driver;

namespace MongoData
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(string connectionString)
            : this(new MongoUrl(connectionString))
        {
        }

        public UnitOfWork(MongoUrl url)
        {
            Client = new MongoClient(url);
            Database = Client.GetDatabase(url.DatabaseName);
        }

        public IMongoClient Client { get; }

        public IMongoDatabase Database { get; }

        public IMongoDatabase CreateNewDatabase()
        {
            throw new NotImplementedException();
        }
    }
}