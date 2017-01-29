using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoData.Tests.Entities
{
    [CollectionName("TestCustomers")]
    public class TestCustomer : Entity
    {
        [BsonElement("fname")]
        public string FirstName { get; set; }

        [BsonElement("lname")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public TestAddress HomeAddress { get; set; }

        public IList<TestOrder> Orders { get; set; }
    }

    public class TestOrder
    {
        public DateTime PurchaseDate { get; set; }

        public IList<TestOrderItem> Items;
    }

    public class TestOrderItem
    {
        public TestProduct Product
        {
            get;
            set;
        }

        public int Quantity { get; set; }
    }

    public class TestAddress
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        [BsonIgnoreIfNull]
        public string Country { get; set; }
    }
}
