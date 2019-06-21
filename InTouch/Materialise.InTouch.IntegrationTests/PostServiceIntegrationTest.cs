using System;
using System.Linq;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Configs;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Services;
using Materialise.InTouch.BLL.Services.PostConfiguration;
using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Interfaces;
using Materialise.InTouch.DAL.Repositories;
using Materialise.InTouch.IntegrationTests.Common;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Materialise.InTouch.IntegrationTests
{
    [Collection("Database collection")]
    public class PostServiceIntegrationTest : IDisposable
    {
        private readonly PostService _postService;

        private readonly PostRepository _postRepository;

        private readonly DatabaseFixture _fixture;
        private readonly InTouchContext _context;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ICommentService> _commentService;
        private readonly Mock<IOptionsSnapshot<PostConfig>> _postOptions;
        private readonly Mock<IOptionsSnapshot<FullscreenBatchConfig>> _batchOptions;

        public PostServiceIntegrationTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.CreateContext();
            _postRepository = new PostRepository(_context);
            TestDataInitializer.ClearData(_context);

            _unitOfWork = new Mock<IUnitOfWork>();
            _userContextMock = new Mock<IUserContext>();
            _fileServiceMock = new Mock<IFileService>();
            _commentService = new Mock<ICommentService>();
            _postOptions = new Mock<IOptionsSnapshot<PostConfig>>();
            _batchOptions = new Mock<IOptionsSnapshot<FullscreenBatchConfig>>();

            _postService = new PostService(_unitOfWork.Object, _fileServiceMock.Object, _commentService.Object, _userContextMock.Object,
                _batchOptions.Object, _postOptions.Object);
            _unitOfWork.SetupGet(x => x.Posts).Returns(_postRepository);

            _batchOptions.SetupProperty(bo => bo.Value.FullscreenBatchSize, 4);

            //_batchOptions.Object.Value.FullscreenBatchSize = 4;
        }

       
        public void Dispose()
        {
            TestDataInitializer.ClearData(_context);
            _context.Dispose();
        }

        [Fact]
        public async Task GetPostsForBatchAsync_should_return_4_posts_with_titles_sequence_OH_3_2_1_when_first_batch_ever()
        {
            //arrange
            await TestDataInitializer.Init_1Post(_context, "FutureNH", -100, PostPriority.High);
            await TestDataInitializer.Init_1Post(_context, "FutureN", -100, PostPriority.Normal);
            await TestDataInitializer.Init_1Post(_context, "3", 10, PostPriority.Normal);
            await TestDataInitializer.Init_1Post(_context, "2", 20, PostPriority.Normal);
            await TestDataInitializer.Init_1Post(_context, "OH", 100, PostPriority.High);
            await TestDataInitializer.Init_1Post(_context, "1", 30, PostPriority.Normal);

            //act
            var result = await _postService.GetPostsForBatchAsync(null,null);

            //assert
            Assert.Equal(result[0].Title, "OH");
            Assert.Equal(result[1].Title, "3");
            Assert.Equal(result[2].Title, "2");
            Assert.Equal(result[3].Title, "1");
        }                                
        [Fact]
        public async Task GetPostsForBatchAsync_should_return_4_posts_with_titles_sequence_NH_OH_N_3_after_first_batch()
        {
            //arrange
            await TestDataInitializer.Init_3HP_3NP_Posts(_context);
            //act
            var result = await _postService.GetPostsForBatchAsync(DateTime.Now.AddDays(-13), _context.Posts.First(p => p.Title == "1").CreatedDate);

            //assert
            Assert.Equal(result[0].Title, "NH");
            Assert.Equal(result[1].Title, "OH");
            Assert.Equal(result[2].Title, "N");
            Assert.Equal(result[3].Title, "3");
        }
        [Fact]
        public async Task GetPostsForBatchAsync_should_return_4_posts_with_titles_sequence_NH_OH_2_1()
        {
            //arrange
            await TestDataInitializer.Init_3HP_3NP_Posts(_context);
            //act
            var result = await _postService.GetPostsForBatchAsync(DateTime.Now.AddDays(-4), _context.Posts.First(p => p.Title == "3").CreatedDate);

            //assert
            Assert.Equal(result[0].Title, "NH");
            Assert.Equal(result[1].Title, "OH");
            Assert.Equal(result[2].Title, "2");
            Assert.Equal(result[3].Title, "1");
        }
        [Fact]
        public async Task GetPostsForBatchAsync_should_return_4_posts_with_titles_sequence_NH_OH_N4_3()
        {
            //arrange
            await TestDataInitializer.Init_3HP_3NP_Posts(_context);
            //act
            var result = await _postService.GetPostsForBatchAsync(DateTime.Now.AddDays(-1), _context.Posts.First(p => p.Title == "1").CreatedDate);

            //assert
            Assert.Equal(result[0].Title, "NH");
            Assert.Equal(result[1].Title, "OH");
            Assert.Equal(result[2].Title, "N");
            Assert.Equal(result[3].Title, "3");
        }

    }
}