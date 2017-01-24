using MongoDB.Driver;

namespace MongoData
{
    public interface IUnitOfWork
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
        IMongoDatabase CreateNewDatabase();
    }
}