namespace MongoData.Tests.Entities
{
    [CollectionName("TestProducts")]
    public class TestProduct : Entity
    {
        public TestProduct()
        {
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
