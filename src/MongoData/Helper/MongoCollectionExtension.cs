using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoData
{
    internal static class MongoCollectionExtension
    {
        public static BsonDocument GetStats<T>(this IMongoCollection<T> src)
        {
            var command = new BsonDocumentCommand<BsonDocument>(new BsonDocument
            {
                {"collstats", src.CollectionNamespace.CollectionName}
            });
            return src.Database.RunCommand(command);
        }

        public static long GetDataSize<T>(this IMongoCollection<T> src)
        {
            var totalSize = src.GetStats()["size"].AsInt64;
            return totalSize;
        }

        public static long GetStorageSize<T>(this IMongoCollection<T> src)
        {
            return src.GetStats()["storageSize"].AsInt64;
        }

        public static long GetIndexsSize<T>(this IMongoCollection<T> src)
        {
            return src.GetStats()["totalIndexSize"].AsInt64;
        }

        public static bool IsCapped<T>(this IMongoCollection<T> src)
        {
            return src.GetStats()["capped"].AsBoolean;
        }
    }
}