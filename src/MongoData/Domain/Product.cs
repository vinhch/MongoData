using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoData.Domain
{
    [CollectionName("Products")]
    public class Product : Entity
    {
        public Product()
        {
            CreatedOn = DateTime.Now;
        }

        public string Name { get; set; }
        [BsonElement("Type")]
        public string TypeStr
        {
            get { return Type?.ToNameString(); }
            private set
            {
                ProductTypes type;
                if (Enum.TryParse(value, true, out type))
                    Type = type;
            }
        }

        [BsonIgnore]
        public ProductTypes? Type { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? Modified { get; set; }
    }

    public enum ProductTypes
    {
        None,
        Product,
        Variant
    }

    public static class ProductTypesExtensions
    {
        public static string ToNameString(this ProductTypes input)
        {
            return input.ToString("F").ToLower();
        }
    }
}