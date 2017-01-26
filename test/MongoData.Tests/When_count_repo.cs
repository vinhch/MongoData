using System;
using System.Collections.Generic;
using System.Linq;
using MongoData.Repository;
using MongoData.Tests.Entities;
using Xunit;

namespace MongoData.Tests
{
    [Collection("Repo test")]
    public class When_count_repo : IDisposable
    {
        private readonly IRepository<TestCustomer> _testCustomerRepo;
        public When_count_repo()
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
        public void Test()
        {
            var count1 = _testCustomerRepo.Count();

            Assert.Equal(7, count1);

            var count2 = _testCustomerRepo.LongCount(s => s.FirstName.Contains("Customer"));

            Assert.Equal(4, count2);
        }
    }
}
