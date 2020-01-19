using MongoData.Repository;
using MongoData.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MongoData.Tests
{
    [Collection("Real Repo test")]
    public class When_delete_repo_async : IDisposable
    {
        private readonly IRepositoryAsync<TestCustomer> _testCustomerRepo;

        public When_delete_repo_async()
        {
            Utils.DropDb();
            _testCustomerRepo = new BaseRepository<TestCustomer>(Utils.MongoUnitOfWork);
        }

        public void Dispose()
        {
            Utils.DropDb();
        }

        [Fact]
        public async Task BatchTestAsync()
        {
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
            _testCustomerRepo.Add(customers);

            Assert.Equal(7, await _testCustomerRepo.CountAsync());

            await _testCustomerRepo.DeleteAsync(s => s.FirstName.Contains("Customer"));

            Assert.Equal(3, await _testCustomerRepo.CountAsync());

            await _testCustomerRepo.DeleteAllAsync();

            Assert.Equal(0, await _testCustomerRepo.CountAsync());
        }

        [Fact]
        public async Task SingleTestAsync()
        {
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

            var cutomer = _testCustomerRepo.SingleOrDefault(s => s.FirstName == "Customer A");
            await _testCustomerRepo.DeleteAsync(cutomer);
            cutomer = _testCustomerRepo.SingleOrDefault(s => s.Id == cutomer.Id);

            Assert.Null(cutomer);

            cutomer = _testCustomerRepo.SingleOrDefault(s => s.FirstName == "Client B");
            await _testCustomerRepo.DeleteAsync(cutomer.Id);
            cutomer = _testCustomerRepo.SingleOrDefault(s => s.Id == cutomer.Id);

            Assert.Null(cutomer);

            await _testCustomerRepo.DeleteAllAsync();
        }
    }
}
