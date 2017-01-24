using System;

namespace MongoData.Helper
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class CollectionName : Attribute
    {
        public CollectionName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty collectionname not allowed", nameof(value));
            Name = value;
        }

        public virtual string Name { get; private set; }
    }
}