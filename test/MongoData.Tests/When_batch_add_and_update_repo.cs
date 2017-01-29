using System;
using System.Collections.Generic;
using MongoData.Repository;
using MongoData.Tests.Entities;
using Xunit;

namespace MongoData.Tests
{
    [Collection("Real Repo test")]
    public class When_batch_add_and_update_repo : IDisposable
    {
        private readonly IRepository<TestCustomer> _testCustomerRepo;

        public When_batch_add_and_update_repo()
        {
            Utils.DropDb();
            _testCustomerRepo = new BaseRepository<TestCustomer>(Utils.MongoUnitOfWork);
        }

        public void Dispose()
        {
            Utils.DropDb();
        }

        [Fact]
        public void Test()
        {
            Assert.False(_testCustomerRepo.Exists());

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

            var count1 = _testCustomerRepo.Count();

            Assert.Equal(7, count1);

            foreach (var customer in customers)
            {
                customer.LastName = customer.FirstName;
            }
            _testCustomerRepo.Update(customers);

            foreach (var customer in _testCustomerRepo)
            {
                Assert.Equal(customer.FirstName, customer.LastName);
            }
        }
    }
}
