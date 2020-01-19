using System;

namespace MongoData
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        public CollectionNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Empty collection name not allowed", nameof(value));
            }
            Name = value;
        }

        public virtual string Name { get; private set; }
    }
}