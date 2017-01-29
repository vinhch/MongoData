using System;
using System.Collections.Generic;
using System.Linq;
using MongoData.Repository;
using MongoData.Tests.Entities;
using Xunit;

namespace MongoData.Tests
{
    [Collection("Real Repo test")]
    public class When_delete_repo : IDisposable
    {
        private readonly IRepository<TestCustomer> _testCustomerRepo;

        public When_delete_repo()
        {
            Utils.DropDb();
            _testCustomerRepo = new BaseRepository<TestCustomer>(Utils.MongoUnitOfWork);
        }

        public void Dispose()
        {
            Utils.DropDb();
        }

        [Fact]
        public void BatchTest()
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

            Assert.Equal(7, _testCustomerRepo.Count());

            _testCustomerRepo.Delete(s => s.FirstName.Contains("Customer"));

            Assert.Equal(3, _testCustomerRepo.Count());

            _testCustomerRepo.DeleteAll();

            Assert.Equal(0, _testCustomerRepo.Count());
        }

        [Fact]
        public void SingleTest()
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

            var cutomer = _testCustomerRepo.SingleOrDefault(s => s.FirstName == "Customer A");
            _testCustomerRepo.Delete(cutomer);
            cutomer = _testCustomerRepo.SingleOrDefault(s => s.Id == cutomer.Id);

            Assert.Null(cutomer);

            cutomer = _testCustomerRepo.SingleOrDefault(s => s.FirstName == "Client B");
            _testCustomerRepo.Delete(cutomer.Id);
            cutomer = _testCustomerRepo.SingleOrDefault(s => s.Id == cutomer.Id);

            Assert.Null(cutomer);

            _testCustomerRepo.DeleteAll();
        }
    }
}
