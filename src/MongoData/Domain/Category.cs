using System;

namespace MongoData.Domain
{
    [CollectionName("Categories")]
    public class Category : Entity
    {
        public Category()
        {
            CreatedOn = DateTime.Now;
        }

        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? Modified { get; set; }
    }
}