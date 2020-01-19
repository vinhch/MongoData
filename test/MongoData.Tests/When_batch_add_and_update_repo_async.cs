using MongoData.Repository;
using MongoData.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MongoData.Tests
{
    [Collection("Real Repo test")]
    public class When_batch_add_and_update_repo_async : IDisposable
    {
        private readonly IRepositoryAsync<TestCustomer> _testCustomerRepo;

        public When_batch_add_and_update_repo_async()
        {
            Utils.DropDb();
            _testCustomerRepo = new BaseRepository<TestCustomer>(Utils.MongoUnitOfWork);
        }

        public void Dispose()
        {
            Utils.DropDb();
        }

        [Fact]
        public async Task TestAsync()
        {
            Assert.False(await _testCustomerRepo.AnyAsync());

            var customers = new List<TestCustomer>
            {
                new TestCustomer {FirstName = "Customer A"},
                new TestCustomer {FirstName = "Client B"},
                new TestCustomer {FirstName = "Customer C"},
                new TestCustomer {FirstName = "Client D"},
                new TestCustomer {FirstName = "Customer E"},
                new TestCustomer {FirstName = "Client F"},
                new TestCustomer {FirstName = "Customer G"}
            };
            await _testCustomerRepo.AddAsync(customers);

            var count1 = await _testCustomerRepo.CountAsync();

            Assert.Equal(7, count1);

            foreach (var customer in customers)
            {
                customer.LastName = customer.FirstName;
            }
            await _testCustomerRepo.UpdateAsync(customers);

            foreach (var customer in _testCustomerRepo)
            {
                Assert.Equal(customer.FirstName, customer.LastName);
            }
        }
    }
}
