using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Contracts.Repositories.Searching;
using Octacom.Odiss.Core.DataLayer.User;

namespace Octacom.Odiss.Core.IntegrationTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private IUserRepository userRepository;

        [TestInitialize]
        public void Initialize()
        {
            userRepository = new UserRepository();
        }

        [TestMethod]
        public void SearchUsers_ItHasRecords()
        {
            var parameters = new UserSearchParameters();

            var result = userRepository.Search(parameters);

            Assert.IsTrue(result.Records.Any());
        }

        [TestMethod]
        public void SearchUsers_EmptyParameters_RecordCountSameAsTotal()
        {
            var parameters = new UserSearchParameters();

            var result = userRepository.Search(parameters);

            Assert.AreEqual(result.Records.Count(), result.TotalCount);
        }

        [TestMethod]
        public void SearchUsers_PageSize2_OnlyTwoRecordsReturnedAndTotalRemainsSame()
        {
            var parameters = new UserSearchParameters
            {
                Page = 1,
                PageSize = 2,
                FirstName = new SearchFilter<string>
                {
                    SortOrder = SortOrder.Ascending
                }
            };

            var pagedResult = userRepository.Search(parameters);

            parameters = new UserSearchParameters();
            var noPageResult = userRepository.Search(parameters);

            Assert.AreEqual(pagedResult.TotalCount, noPageResult.TotalCount);
            Assert.AreNotEqual(pagedResult.Records.Count(), noPageResult.Records.Count());
        }

        [TestMethod]
        public void SearchUsers_FilterByUserName_ResultsOnlyWithUsernameContaining()
        {
            var parameters = new UserSearchParameters
            {
                UserName = new SearchFilter<string>
                {
                    Value = "oct",
                    FilterType = FilterType.Like
                }
            };

            var result = userRepository.Search(parameters);

            Assert.IsTrue(result.FilteredCount > 0);
            Assert.IsTrue(result.Records.All(x => x.UserName.Contains(parameters.UserName.Value)));
        }

        [TestMethod]
        public void SearchUsers_FilterStartsWithUsername_ResultsOnlyWithUsernameStartsWith()
        {
            var parameters = new UserSearchParameters
            {
                UserName = new SearchFilter<string>
                {
                    Value = "oct",
                    FilterType = FilterType.StartsWith
                }
            };

            var result = userRepository.Search(parameters);

            Assert.IsTrue(result.FilteredCount > 0);
            Assert.IsTrue(result.Records.All(x => x.UserName.StartsWith(parameters.UserName.Value)));
        }

        [TestMethod]
        public void SearchUsers_FilterEndsWithUsername_ResultsOnlyWithUsernameEndsWith()
        {
            var parameters = new UserSearchParameters
            {
                UserName = new SearchFilter<string>
                {
                    Value = "com",
                    FilterType = FilterType.EndsWith
                }
            };

            var result = userRepository.Search(parameters);

            Assert.IsTrue(result.FilteredCount > 0);
            Assert.IsTrue(result.Records.All(x => x.UserName.EndsWith(parameters.UserName.Value)));
        }

        [TestMethod]
        public void SearchUsers_FilterTypeGreaterThanZero_ResultsAllGreaterThanZero()
        {
            var parameters = new UserSearchParameters
            {
                Type = new SearchFilter<byte?>
                {
                    Value = 0,
                    FilterType = FilterType.GreaterThan
                }
            };

            var result = userRepository.Search(parameters);

            Assert.IsTrue(result.FilteredCount > 0);
            Assert.IsTrue(result.Records.All(x => x.Type > 0));
        }

        [TestMethod]
        public void SearchUsers_FilterTypeLessThanFour_ResultsAllLessThanFour()
        {
            var parameters = new UserSearchParameters
            {
                Type = new SearchFilter<byte?>
                {
                    Value = 4,
                    FilterType = FilterType.LessThan
                }
            };

            var result = userRepository.Search(parameters);

            Assert.IsTrue(result.FilteredCount > 0);
            Assert.IsTrue(result.Records.All(x => x.Type < 4));
        }

        [TestMethod]
        public void SearchUsers_SortByUserNameDesc_ResultsSortedByUserNameDescending()
        {
            var parameters = new UserSearchParameters
            {
                LastName = new SearchFilter<string>
                {
                    SortOrder = SortOrder.Descending
                }
            };

            var result = userRepository.Search(parameters);
            var resultLastNames = result.Records.Select(x => x.LastName).ToList();
            var inMemoryDescSorted = resultLastNames.OrderByDescending(x => x).ToList();

            Assert.IsTrue(resultLastNames.SequenceEqual(inMemoryDescSorted));
        }

        [TestMethod]
        public void GetByUsername_ReturnsRecord()
        {
            var result = userRepository.GetByUsername("octacom");

            Assert.IsNotNull(result.UserName);
            Assert.AreEqual(result.UserName, "octacom");
        }
    }
}
