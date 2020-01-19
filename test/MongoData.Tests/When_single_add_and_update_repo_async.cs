using MongoData.Repository;
using MongoData.Tests.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MongoData.Tests
{
    [Collection("Real Repo test")]
    public class When_single_add_and_update_repo_async : IDisposable
    {
        private readonly IRepositoryAsync<TestProduct> _testProductRepo;
        public When_single_add_and_update_repo_async()
        {
            Utils.DropDb();
            _testProductRepo = new BaseRepository<TestProduct>(Utils.MongoUnitOfWork);
        }

        public void Dispose()
        {
            Utils.DropDb();
        }

        [Fact]
        public async Task TestAsync()
        {
            Assert.False(await _testProductRepo.AnyAsync());

            var product = new TestProduct
            {
                Name = "Fristi Milk Async",
                Description = "Orange Flavor",
                Price = 10000
            };
            await _testProductRepo.AddAsync(product);

            Assert.True(await _testProductRepo.AnyAsync());
            Assert.NotNull(product.Id);

            var alreadyAddedProduct = _testProductRepo.SingleOrDefault(c => c.Name == "Fristi Milk Async");

            Assert.NotNull(alreadyAddedProduct);
            Assert.Equal(product.Id, alreadyAddedProduct.Id);

            alreadyAddedProduct.Description = "Lemon Flavor";
            await _testProductRepo.UpdateAsync(alreadyAddedProduct);
            var updatedProduct = await _testProductRepo.GetByIdAsync(product.Id);

            Assert.NotNull(updatedProduct);
            Assert.Equal(product.Id, updatedProduct.Id);
            Assert.Equal("Lemon Flavor", updatedProduct.Description);
        }
    }
}
