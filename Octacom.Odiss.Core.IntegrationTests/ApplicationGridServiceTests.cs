using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Octacom.Odiss.Core.Business;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Odiss.Core.IntegrationTests.TestImplementations;
using Octacom.Odiss.Core.DataLayer;
using Octacom.Odiss.Core.DataLayer.Application;

namespace Octacom.Odiss.Core.IntegrationTests
{
    [TestClass]
    public class ApplicationGridServiceTests
    {
        private IApplicationGridService service;
        private ItemNumberRepository itemNumberRepository;

        [TestInitialize]
        public void Initialize()
        {
            var configService = new ConfigService(
                new SettingsRepository(),
                new ApplicationRepository(),
                new FieldRepository(),
                new DatabaseRepository(),
                new CachingServiceMock()
                );

            var applicationService = new SimpleApplicationService();

            service = new ApplicationGridService(configService, applicationService);
            itemNumberRepository = new ItemNumberRepository();
        }

        [TestMethod]
        public void MapDataToEntity_ReturnsEntityWithCorrespondingValues()
        {
            var appId = new Guid("E8349DE1-65E4-E811-8226-D89EF34A256D");
            var id = Guid.NewGuid();

            var item = new Dictionary<Guid, object>
            {
                { new Guid("FF9264C6-52E8-E811-8229-D89EF34A256D"), id },
                { new Guid("82497bb3-98e6-e811-8226-d89ef34a256d"), "Vendor Number" },
                { new Guid("83497bb3-98e6-e811-8226-d89ef34a256d"), "Vendor Name" },
                { new Guid("84497bb3-98e6-e811-8226-d89ef34a256d"), "Inv Item Number" },
                { new Guid("85497bb3-98e6-e811-8226-d89ef34a256d"), "JDE" }
            };

            var result = service.MapDataToEntity<ItemNumber>(appId, item);

            Assert.AreEqual(id, result.Id);
            Assert.AreEqual("Vendor Number", result.VendorNumber);
            Assert.AreEqual("Vendor Name", result.VendorName);
            Assert.AreEqual("Inv Item Number", result.InvoiceItemNumber);
            Assert.AreEqual("JDE", result.JDENumber);
        }

        [TestMethod]
        public void MapDataToEntity_CanUseStringGuidRepresentationInDictionaryData()
        {
            var appId = new Guid("E8349DE1-65E4-E811-8226-D89EF34A256D");
            var id = Guid.NewGuid().ToString();

            var item = new Dictionary<Guid, object>
            {
                { new Guid("FF9264C6-52E8-E811-8229-D89EF34A256D"), id },
                { new Guid("82497bb3-98e6-e811-8226-d89ef34a256d"), "Vendor Number" },
                { new Guid("83497bb3-98e6-e811-8226-d89ef34a256d"), "Vendor Name" },
                { new Guid("84497bb3-98e6-e811-8226-d89ef34a256d"), "Inv Item Number" },
                { new Guid("85497bb3-98e6-e811-8226-d89ef34a256d"), "JDE" }
            };

            var result = service.MapDataToEntity<ItemNumber>(appId, item);

            Assert.AreEqual(Guid.Parse(id), result.Id);
            Assert.AreEqual("Vendor Number", result.VendorNumber);
            Assert.AreEqual("Vendor Name", result.VendorName);
            Assert.AreEqual("Inv Item Number", result.InvoiceItemNumber);
            Assert.AreEqual("JDE", result.JDENumber);
        }

        [TestMethod]
        public void MapEntityToData_ReturnsDictionaryWithEntityValues()
        {
            var appId = new Guid("E8349DE1-65E4-E811-8226-D89EF34A256D");
            var id = Guid.NewGuid();

            var itemNumber = new ItemNumber
            {
                Id = id,
                VendorNumber = "Vendor Number",
                VendorName = "Vendor Name",
                InvoiceItemNumber = "Inv Item Number",
                JDENumber = "JDE"
            };

            var result = service.MapEntityToData(appId, itemNumber);

            Assert.AreEqual(id, result[new Guid("FF9264C6-52E8-E811-8229-D89EF34A256D")]);
            Assert.AreEqual("Vendor Number", result[new Guid("82497bb3-98e6-e811-8226-d89ef34a256d")]);
            Assert.AreEqual("Vendor Name", result[new Guid("83497bb3-98e6-e811-8226-d89ef34a256d")]);
            Assert.AreEqual("Inv Item Number", result[new Guid("84497bb3-98e6-e811-8226-d89ef34a256d")]);
            Assert.AreEqual("JDE", result[new Guid("85497bb3-98e6-e811-8226-d89ef34a256d")]);
        }

        [TestMethod]
        public void ResolveFieldFilter_View_ResultIsNotNull()
        {
            var fieldId = new Guid("ABD417D7-B112-E911-842C-005056820BD7");

            var result = service.ResolveFieldFilter(fieldId, null);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ResolveFieldFilter_Rest_ResultIsNotNull()
        {
            var fieldId = new Guid("856183AE-E40A-E911-842C-005056820BD7");

            var result = service.ResolveFieldFilter(fieldId, null);

            Assert.IsNotNull(result);
        }
    }
}
