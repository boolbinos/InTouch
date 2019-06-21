using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Services;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Infrastructure;
using Materialise.InTouch.DAL.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Materialise.InTouch.BLL.Configs;
using Materialise.InTouch.BLL.Services.PostConfiguration;
using Microsoft.Extensions.Options;
using Xunit;
using Materialise.InTouch.BLL.ModelsDTO;

namespace Materialise.InTouch.Tests.Materialise.InTouch.BLL.Tests
{
    public class PostServiceTest
    {
        private readonly PostService _postService;

        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ICommentService> _commentService;
        private readonly Mock<IOptionsSnapshot<PostConfig>> _options;
        private readonly Mock<IOptionsSnapshot<FullscreenBatchConfig>> _batchOptions;

        public PostServiceTest()
        {
            _batchOptions = new Mock<IOptionsSnapshot<FullscreenBatchConfig>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _userContextMock = new Mock<IUserContext>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _fileServiceMock = new Mock<IFileService>();
            _commentService = new Mock<ICommentService>();
            _options = new Mock<IOptionsSnapshot<PostConfig>>();
            _postService = new PostService(_unitOfWork.Object, _fileServiceMock.Object, _commentService.Object, _userContextMock.Object, _batchOptions.Object, _options.Object);
            _unitOfWork.SetupGet(x => x.Posts).Returns(_postRepositoryMock.Object);
        }

        [Fact]
        public async Task FindAsync_should_return_2_items_when_database_have_2_items()
        {
            //arrange
            _postRepositoryMock.Setup(r => r.FindValidAsync(It.IsAny<Expression<Func<Post, bool>>>()))
                .ReturnsAsync(new List<Post>()
                {
                    new Post(),
                    new Post()
                });

            //act
            var result = await _postService.FindValidAsync(i => i.IsDeleted);

            //assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetPageAsync_should_return_2items_with_Id3_Id4_when_page2_and_pageSize2()
        {
            //arrange
            _postRepositoryMock.Setup(p => p.GetPageAsync(2, 2, 2, false, null))
                .ReturnsAsync(new PagedResult<Post>(new List<Post>
                {
                    new Post() {Id = 3, User=new User()},
                    new Post() {Id = 4, User=new User()}
                }, 1, 10, 100));


            //act
            var result = await _postService.GetPageAsync(2, 2, 2, false, null);
            var post1 = result.Data.ElementAt(0);
            var post2 = result.Data.ElementAt(1);

            //assert
            Assert.Equal(3, post1.Id);
            Assert.Equal(4, post2.Id);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task CreateAsync_should_throw_InvalidOperationException_when_postEndDate_less_postStartDate()
        {
            //arrange
            var postDTO = new PostDTO() { StartDateTime = new DateTime(2017, 10, 11), EndDateTime = new DateTime(2017, 10, 10) };

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _postService.CreateAsync(postDTO)
            );
            //assert
            Assert.Equal("EndDateTime should be greater than StartDateTime", exception.Message);

        }


        class PostContext
        {
            public List<Post> Posts { get; }

            public PostContext()
            {
                Posts = new List<Post>
                {
                    new Post{
                    Id = 1,
                    Content = "Content1",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsPublic = true,
                    Title = "Title1",
                    UserId = 1
                    },
                    new Post{
                    Id = 2,
                    Content = "Content2",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsPublic = true,
                    Title = "Title2",
                    UserId = 1
                    },
                    new Post{
                    Id = 3,
                    Content = "Content3",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsPublic = true,
                    Title = "Title3",
                    UserId = 1
                    },
                    new Post{
                    Id = 4,
                    Content = "Content4",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsPublic = true,
                    Title = "Title4",
                    UserId = 1
                    }
                };
            }
        }
    }
}
