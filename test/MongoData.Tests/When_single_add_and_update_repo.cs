using System;
using System.Linq;
using MongoData.Repository;
using MongoData.Tests.Entities;
using Xunit;

namespace MongoData.Tests
{
    [Collection("Real Repo test")]
    public class When_single_add_and_update_repo : IDisposable
    {
        private readonly IRepository<TestProduct> _testProductRepo;
        public When_single_add_and_update_repo()
        {
            Utils.DropDb();
            _testProductRepo = new BaseRepository<TestProduct>(Utils.MongoUnitOfWork);
        }

        public void Dispose()
        {
            Utils.DropDb();
        }

        [Fact]
        public void Test()
        {
            Assert.False(_testProductRepo.Exists());

            var product = new TestProduct
            {
                Name = "Fristi Milk",
                Description = "Orange Flavor",
                Price = 10000
            };
            _testProductRepo.Add(product);

            Assert.True(_testProductRepo.Exists());
            Assert.NotNull(product.Id);

            var alreadyAddedProduct = _testProductRepo.SingleOrDefault(c => c.Name == "Fristi Milk");

            Assert.NotNull(alreadyAddedProduct);
            Assert.Equal(product.Id, alreadyAddedProduct.Id);

            alreadyAddedProduct.Description = "Lemon Flavor";
            _testProductRepo.Update(alreadyAddedProduct);
            var updatedProduct = _testProductRepo.GetById(product.Id);

            Assert.NotNull(updatedProduct);
            Assert.Equal(product.Id, updatedProduct.Id);
        }
    }
}
