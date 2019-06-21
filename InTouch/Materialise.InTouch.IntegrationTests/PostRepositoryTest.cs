using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Repositories;
using Materialise.InTouch.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Materialise.InTouch.DAL.Entities;
using Xunit;

namespace Materialise.InTouch.IntegrationTests
{
    [Collection("Database collection")]
    public class PostRepositoryTest : IDisposable
    {
        private readonly PostRepository _postRepository;

        private readonly DatabaseFixture _fixture;
        private readonly InTouchContext _context;

        public PostRepositoryTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.CreateContext();
            _postRepository = new PostRepository(_context);
            TestDataInitializer.ClearData(_context);
        }

        public void Dispose()
        {
            TestDataInitializer.ClearData(_context);
            _context.Dispose();
        }


 

        [Fact]
        //property isDeleted == true where post Id == 5 
        public async Task GetAsync_should_return_null_when_postIsDeleted_true()
        {
            //arrange
            var post = await TestDataInitializer.InitOneDeletedPostAsync(_context);

            //act
            var result = await _postRepository.GetAsync(post.Id);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_should_return_4_posts_when_database_has_5_posts_with_1_deleted_Post()
        {
            // arrange          
            var user = await TestDataInitializer.InitOneGeneralUserAsync(_context);
            await TestDataInitializer.Init5PostsWith1DeletedPost(_context, user);

            //act
            var result = await _postRepository.GetAllAsync();

            //assert
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GetPageAsync_should_return_2_posts_when_page_is_2_and_pageSize_is_2()
        {
            // arrange
            var user = await TestDataInitializer.InitOneGeneralUserAsync(_context);
            await TestDataInitializer.Init5PostsWith1DeletedPost(_context, user);

            //act
            var result = await _postRepository.GetPageAsync(1, 2, 2, false, null);

            //assert
            Assert.Equal(2, result.Data.Count);
        }

        [Fact]
        public async Task FindAsync_should_return_null_if_post_is_deleted()
        {
            // arrange          
            var user = await TestDataInitializer.InitOneGeneralUserAsync(_context);
            await TestDataInitializer.Init5PostsWith1DeletedPost(_context, user);

            //act
            var result = await _postRepository.FindValidAsync(p => p.IsDeleted);

            //assert
            Assert.Equal(0, result.Count);
        }

    }
}
