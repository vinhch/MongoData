using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoData
{
    [DataContract]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Entity : IEntity
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}