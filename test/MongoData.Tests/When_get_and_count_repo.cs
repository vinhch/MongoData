using System;
using System.Collections.Generic;
using System.Linq;
using MongoData.Repository;
using MongoData.Tests.Entities;
using Xunit;

namespace MongoData.Tests
{
    [Collection("Real Repo test")]
    public class When_get_and_count_repo : IDisposable
    {
        private readonly IRepository<TestCustomer> _testCustomerRepo;
        public When_get_and_count_repo()
        {
            Utils.DropDb();
            _testCustomerRepo = new BaseRepository<TestCustomer>(Utils.MongoUnitOfWork);

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
        }

        public void Dispose()
        {
            Utils.DropDb();
        }

        [Fact]
        public void GetTest()
        {
            var customer = _testCustomerRepo.SingleOrDefault(s => s.FirstName == "Customer A");

            Assert.NotNull(customer);
            Assert.Equal("Customer A", customer.FirstName);

            var customer1 = _testCustomerRepo.GetById(customer.Id);

            Assert.NotNull(customer1);
            Assert.Equal(customer.Id, customer1.Id);
        }

        [Fact]
        public void CountTest()
        {
            Assert.Equal(7, _testCustomerRepo.Count());

            Assert.Equal(4, _testCustomerRepo.LongCount(s => s.FirstName.Contains("Customer")));
        }
    }
}
