using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octacom.DapperRepository;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.DataLayer.Application;
using Octacom.Odiss.Core.IntegrationTests.TestImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octacom.Odiss.Core.IntegrationTests
{
    // NOTE - This is not a automated test. It has hard coded guid for application ID. For the time being, if you want this to work then update the Guid with some specific to your application.
    // I highly recommend auto initializing this test in the future (if CI becomes a real thing at Octacom).

    [TestClass]
    public class ApplicationGridRepositoryTest
    {
        private IApplicationGridRepository repository;
        private ItemNumberRepository itemNumberRepository;

        [TestInitialize]
        public void Initialize()
        {
            repository = new ApplicationGridRepository(new ApplicationRepository(), new DatabaseRepository(), new FieldRepository(), null);
            itemNumberRepository = new ItemNumberRepository();
        }

        [TestMethod]
        public void GetAll_ReturnsDataRows()
        {
            var data = repository.GetAll(new Guid("E8349DE1-65E4-E811-8226-D89EF34A256D"));

            Assert.IsTrue(data.Any());
        }

        [TestMethod]
        public void Insert_ItemIsInserted()
        {
            var appId = new Guid("E8349DE1-65E4-E811-8226-D89EF34A256D");

            var item = new Dictionary<Guid, object>
            {
                { new Guid("82497bb3-98e6-e811-8226-d89ef34a256d"), "12345" },
                { new Guid("83497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
                { new Guid("84497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
                { new Guid("85497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
            };

            var created = repository.Insert(appId, item);

            var createdItem = itemNumberRepository.Get(created.Id);

            Assert.AreEqual("12345", createdItem.VendorNumber);
        }

        [TestMethod]
        public void Update_ItemHasUpdatedFields()
        {
            var appId = new Guid("E8349DE1-65E4-E811-8226-D89EF34A256D");

            var itemNumber = new ItemNumber
            {
                InvoiceItemNumber = "1234",
                VendorName = "Bob's Burgers",
                VendorNumber = "1",
                JDENumber = "Bob"
            };

            var key = itemNumberRepository.Insert(itemNumber).key;

            var item = new Dictionary<Guid, object>
            {
                { new Guid("82497bb3-98e6-e811-8226-d89ef34a256d"), "12345" },
                { new Guid("83497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
                { new Guid("84497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
                { new Guid("85497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
                { new Guid("ff9264c6-52e8-e811-8229-d89ef34a256d"), key }
            };

            repository.Update(appId, item);

            var updatedItem = itemNumberRepository.Get(key);

            Assert.AreEqual("Bob", updatedItem.JDENumber);
        }

        [TestMethod]
        public void Update_ItemWillNotUpdateNonEditableFields()
        {
            var appId = new Guid("E8349DE1-65E4-E811-8226-D89EF34A256D");

            var itemNumber = new ItemNumber
            {
                InvoiceItemNumber = "1234",
                VendorName = "Bob's Burgers",
                VendorNumber = "1",
                JDENumber = "Bob"
            };

            var key = itemNumberRepository.Insert(itemNumber).key;

            var item = new Dictionary<Guid, object>
            {
                { new Guid("82497bb3-98e6-e811-8226-d89ef34a256d"), "12345" },
                { new Guid("83497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
                { new Guid("84497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
                { new Guid("85497bb3-98e6-e811-8226-d89ef34a256d"), "Bob" },
                { new Guid("ff9264c6-52e8-e811-8229-d89ef34a256d"), key }
            };

            repository.Update(appId, item);

            var updatedItem = itemNumberRepository.Get(key);

            Assert.AreNotEqual("12345", updatedItem.VendorNumber);
        }

        [TestMethod]
        public void Delete_ItNoLongerExists()
        {
            var appId = new Guid("E8349DE1-65E4-E811-8226-D89EF34A256D");

            var itemNumber = new ItemNumber
            {
                InvoiceItemNumber = "1234",
                VendorName = "Bob's Burgers",
                VendorNumber = "1",
                JDENumber = "Bob"
            };

            var key = itemNumberRepository.Insert(itemNumber).key;

            repository.Delete(appId, key);

            var getDeletedItemResult = itemNumberRepository.Get(key);

            Assert.IsNull(getDeletedItemResult);
        }
    }
}
