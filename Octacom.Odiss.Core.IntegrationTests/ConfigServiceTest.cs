using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octacom.Odiss.Core.Business;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Odiss.Core.DataLayer;
using Octacom.Odiss.Core.DataLayer.Application;

namespace Octacom.Odiss.Core.IntegrationTests
{
    [TestClass]
    public class ConfigServiceTest
    {
        private IConfigService configService;

        [TestInitialize]
        public void Initialize()
        {
            configService = new ConfigService(
                new SettingsRepository(),
                new ApplicationRepository(),
                new FieldRepository(),
                new DatabaseRepository(),
                new CachingServiceMock()
                );
        }

        [TestMethod]
        public void GetApplicationSettings_ReturnsSettingsData()
        {
            var data = configService.GetApplicationSettings();

            Assert.IsNotNull(data.Name);
            Assert.IsNotNull(data.EnabledLanguages);
            Assert.IsTrue(data.EnabledLanguages.Length == 2);
            Assert.IsTrue(data.MaximumPasswordLength > 0);
        }

        [TestMethod]
        public void GetApplications_ReturnsData()
        {
            var data = configService.GetApplications();

            Assert.IsTrue(data.Any());
        }
    }
}
