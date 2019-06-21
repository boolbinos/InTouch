using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Repositories;
using Materialise.InTouch.IntegrationTests.Common;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Materialise.InTouch.IntegrationTests
{
    [Collection("Database collection")]
    public class UserRepositoryTest : IDisposable
    {
        private readonly UserRepository _userRepository;

        private readonly DatabaseFixture _fixture;
        private readonly InTouchContext _context;

        public UserRepositoryTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.CreateContext();
            _userRepository = new UserRepository(_context);
            TestDataInitializer.ClearData(_context);
        }

        public void Dispose()
        {
            TestDataInitializer.ClearData(_context);
            _context.Dispose();
        }

        [Fact]
        public async Task GetAsync_should_return_null_when_userIsDeleted_true()
        {
            //arrange
            var user = await TestDataInitializer.InitOneDeletedUserAsync(_context);

            //act
            var result = await _userRepository.GetAsync(user.Id);

            //assert
            Assert.Null(result);
        }

        [Fact]

        public async Task GetAllValidAsync_should_return_2_users_when_database_has_3_users_with_1_deleted_user()
        {
            // arrange          
            TestDataInitializer.InitThreeUsersWhereLastOneIsDeleted(_context);

            //act
            var result = await _userRepository.GetAllValidAsync();

            //assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPageAsync_should_return_1_user_when_page_is_3_and_pageSize_is_2()
        {
            // arrange          
            TestDataInitializer.Init6UsersWhereLastOneIsDeleted(_context);

            //act
            var result = await _userRepository.GetPageAsync(3, 2);

            //assert
            Assert.Equal(1, result.Data.Count);
        }

    }
}
