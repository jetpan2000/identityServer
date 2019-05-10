using System;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Octacom.Odiss.Core.Business;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Odiss.Core.DataLayer.User;
using Octacom.Odiss.Core.Entities.User;
using Octacom.Odiss.Core.IntegrationTests.TestImplementations;

namespace Octacom.Odiss.Core.IntegrationTests
{
    [TestClass]
    public class UserServiceTest
    {
        private IUserService userService;
        private IUserRepository userRepository;

        private Mock<IUserRepository> mockUserRepository;
        private Mock<IPrincipalService> mockPrincipalService;
        private IUserService userServiceWithMockRepository;

        const string MY_CUST = "mycust";

        [TestInitialize]
        public void Initialize()
        {
            mockPrincipalService = new Mock<IPrincipalService>();
            userRepository = new UserRepository();
            userService = new UserService(userRepository, mockPrincipalService.Object);
            mockUserRepository = new Mock<IUserRepository>();
            userServiceWithMockRepository = new UserService(mockUserRepository.Object, mockPrincipalService.Object);
        }

        [TestMethod]
        public void UnlockUser_Success()
        {
            var user = userRepository.GetByUsername(MY_CUST);
            LockUser(user);

            var result = userService.UnlockUser(user.Id);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void UnlockUser_ClearsWrongPasswordAttempts()
        {
            var user = userRepository.GetByUsername(MY_CUST);
            LockUser(user);

            userService.UnlockUser(user.Id);

            // Refresh user
            user = userRepository.GetByUsername(MY_CUST);
            Assert.IsNull(user.WrongAccessAttempts);
        }

        [TestMethod]
        public void UnlockUser_ClearsLockAccessUntilField()
        {
            var user = userRepository.GetByUsername(MY_CUST);
            LockUser(user);

            userService.UnlockUser(user.Id);

            // Refresh user
            user = userRepository.GetByUsername(MY_CUST);
            Assert.IsNull(user.LockAccessUntil);
        }

        private void LockUser(User user)
        {
            user.WrongAccessAttempts = 3;
            user.LockAccessUntil = DateTime.Now.AddMonths(1);

            userRepository.Update(user, user.Id);
        }

        [TestMethod]
        public void HasPermission_IfExistsThenItReturnsTrue()
        {
            var principal = new GenericPrincipal(new GenericIdentity("TestUser"), new string[0]);

            mockPrincipalService.Setup(x => x.GetCurrentPrincipal()).Returns(principal);

            mockUserRepository.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(new User
            {
                Permissions = UserPermission.ViewDocuments | UserPermission.PrintDocuments,
                Type = (byte)UserType.Regular
            });

            var result = userServiceWithMockRepository.HasPermission<UserPermission>( UserPermission.ViewDocuments);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasPermission_IfNotExistsThenItReturnsFalse()
        {
            var principal = new GenericPrincipal(new GenericIdentity("TestUser"), new string[0]);

            mockPrincipalService.Setup(x => x.GetCurrentPrincipal()).Returns(principal);

            mockUserRepository.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(new User
            {
                Permissions = UserPermission.ViewDocuments | UserPermission.PrintDocuments,
                Type = (byte)UserType.Regular
            });

            var result = userServiceWithMockRepository.HasPermission<UserPermission>(UserPermission.DeleteNotes);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasPermission_WithCustomPermissionsIfExistsThenItReturnsTrue()
        {
            var principal = new GenericPrincipal(new GenericIdentity("TestUser"), new string[0]);

            mockPrincipalService.Setup(x => x.GetCurrentPrincipal()).Returns(principal);

            mockUserRepository.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(new User
            {
                Permissions = (UserPermission)(CustomUserPermission.BrewCoffee) | (UserPermission)(CustomUserPermission.SendFaxes),
                Type = (byte)UserType.Regular
            });

            var result = userServiceWithMockRepository.HasPermission<CustomUserPermission>(CustomUserPermission.BrewCoffee);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasPermission_WithCustomPermissionsIfNotExistsThenItReturnsFalse()
        {
            var principal = new GenericPrincipal(new GenericIdentity("TestUser"), new string[0]);

            mockPrincipalService.Setup(x => x.GetCurrentPrincipal()).Returns(principal);

            mockUserRepository.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(new User
            {
                Permissions = (UserPermission)(CustomUserPermission.BrewCoffee) | (UserPermission)(CustomUserPermission.SendFaxes),
                Type = (byte)UserType.Regular
            });

            var result = userServiceWithMockRepository.HasPermission(CustomUserPermission.DeleteNotes);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasPermission_WithCustomUserTypeIfExistsThenItReturnsTrue()
        {
            var principal = new GenericPrincipal(new GenericIdentity("TestUser"), new string[0]);

            mockPrincipalService.Setup(x => x.GetCurrentPrincipal()).Returns(principal);

            mockUserRepository.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(new User
            {
                Permissions = (UserPermission)(CustomUserPermission.BrewCoffee) | (UserPermission)(CustomUserPermission.SendFaxes),
                Type = (byte)CustomUserType.AlmightyGod
            });

            var result = userServiceWithMockRepository.HasPermission(CustomUserPermission.BrewCoffee);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasPermission_WithCustomUserTypeIfNotExistsThenItReturnsFalse()
        {
            var principal = new GenericPrincipal(new GenericIdentity("TestUser"), new string[0]);

            mockPrincipalService.Setup(x => x.GetCurrentPrincipal()).Returns(principal);

            mockUserRepository.Setup(x => x.GetByUsername(It.IsAny<string>())).Returns(new User
            {
                Permissions = (UserPermission)(CustomUserPermission.BrewCoffee) | (UserPermission)(CustomUserPermission.SendFaxes),
                Type = (byte)CustomUserType.AlmightyGod
            });

            var result = userServiceWithMockRepository.HasPermission(CustomUserPermission.DeleteNotes);

            Assert.IsFalse(result);
        }
    }
}
