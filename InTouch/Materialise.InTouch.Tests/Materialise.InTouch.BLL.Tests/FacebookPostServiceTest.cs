using Materialise.InTouch.BLL.Providers.ExternalPostProviders;
using Materialise.InTouch.BLL.Services;
using Materialise.InTouch.DAL.Entities;
using Materialise.InTouch.DAL.Interfaces;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Castle.Core.Logging;
using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook;
using Materialise.InTouch.BLL.Mappers;
using Materialise.InTouch.BLL.ModelsDTO;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook.Models;
using Materialise.InTouch.BLL.Services.Storage.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Materialise.InTouch.BLL.Services.PostConfiguration;

namespace Materialise.InTouch.Tests.Materialise.InTouch.BLL.Tests
{
    public class FacebookPostServiceTest
    {
        private readonly Mock<ILogger<ExternalPostService>> _logger;
        private readonly ExternalPostService _externalPostService;
        private readonly FacebookPostCreator _fbPostCreator;

        private readonly Mock<IExternalPostProviderFactory> _providerFactoryMock;
        private readonly Mock<IPostService> _postService;
        private readonly Mock<IFacebookClient> _facebookClient;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWork;


        public FacebookPostServiceTest()
        {
            var config = new PostConfig()
            {
                DurationInSeconds = 10,
                DisplayPeriodInDaysForAdmins = 10
            };


            var postOptions = new Mock<IOptionsSnapshot<PostConfig>>();
            postOptions.Setup(m => m.Value).Returns(config);


            _unitOfWork = new Mock<IUnitOfWork>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _providerFactoryMock = new Mock<IExternalPostProviderFactory>();
            _postService = new Mock<IPostService>();
            _facebookClient = new Mock<IFacebookClient>();
            _fileService = new Mock<IFileService>();
            _unitOfWork.SetupGet(x => x.Posts).Returns(_postRepositoryMock.Object);
            _logger = new Mock<ILogger<ExternalPostService>>();
            _externalPostService = new ExternalPostService(_providerFactoryMock.Object, _unitOfWork.Object, _logger.Object, postOptions.Object);
            _fbPostCreator = new FacebookPostCreator(_facebookClient.Object, _fileService.Object);
        }

        [Fact]
        public async Task ImportPostsAsync_throws_argument_null_exception_if_provider_name_is_null_or_empty()
        {
            //arrange

            //act
            var exception = await Record.ExceptionAsync(() => _externalPostService.ImportPostsAsync(""));

            //assert
            Assert.IsType(typeof(ArgumentNullException), exception);
            Assert.Contains("Invalid parameter", exception.Message);
        }

        [Fact]
        public async Task ImportPostsAsync_throws_argument_exception_if_provider_name_contains_invalid_characters()
        {
            //arrange

            //act
            var exception = await Record.ExceptionAsync(() => _externalPostService.ImportPostsAsync("123F"));

            //assert
            Assert.IsType(typeof(ArgumentException), exception);
            Assert.Contains("Invalid characters", exception.Message);
        }

        [Fact]
        public async Task ImportPostsAsync_throws_argument_exception_if_provider_is_unknown()
        {
            //arrange

            //act
            var exception = await Record.ExceptionAsync(() => _externalPostService.ImportPostsAsync("LinkedIn"));

            //assert
            Assert.IsType(typeof(ArgumentException), exception);
            Assert.Contains("Unknown provider", exception.Message);
        }

        [Fact]
        public async Task ImportPostsAsync_adds_the_same_number_of_posts_as_receives_from_facebook_provider()
        {
            //arrange 
            _postRepositoryMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Post, bool>>>()))
                .ReturnsAsync(new List<Post>()
                {
                    new Post
                    {
                        CreatedDate = DateTime.Parse("28 July 2017"),
                    }
                });

            _providerFactoryMock.Setup(r => r.GetFacebookPostProvider()
                .GetPostsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Post>()
                {
                    new Post(),
                    new Post()
                });

            //act
            var result = await _externalPostService.ImportPostsAsync("facebook");

            //assert
            Assert.Equal("2", result.ToString());
        }

        [Fact]
        public void Mapper_ConvertFacebookPostToPost_should_throw_argument_null_exception_when_input_argument_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => ExternalPostMapper.ConvertFacebookPostToPost(null, null));
        }

        [Fact]
        public async Task GetPageName_throws_argument_null_exception_if_page_id_is_not_defined()
        {
            //arrange

            //act
            var exception = await Record.ExceptionAsync(() => _fbPostCreator.GetPageName(""));

            //assert
            Assert.IsType(typeof(ArgumentNullException), exception);
            Assert.Contains("PageId is not defined.", exception.Message);
        }

        [Fact]
        public async Task GetPostImages_returns_empty_list_if_subattachments_and_media_fields_are_null()
        {
            //arrange
            AttachmentData attachments = new AttachmentData
            {
                Subattachments = null,
                Media = null
            };
            //act
            var list = await _fbPostCreator.GetPostImages(attachments, It.IsAny<string>());

            //assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task GetPostImages_returns_empty_list_if_subattachments_field_is_null_and_image_in_media_is_null()
        {
            //arrange
            AttachmentData attachments = new AttachmentData
            {
                Subattachments = null,
                Media = new Media
                {
                    Image = null
                }
            };
            //act
            var list = await _fbPostCreator.GetPostImages(attachments, It.IsAny<string>());

            //assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task GetPostImages_returns_list_with_one_image_if_subattachments_field_is_null_and_media_field_contains_one_image()
        {
            //arrange
            _fileService.Setup(f => f.CreateAsync(It.IsAny<CreateFileRequest>(), It.IsAny<Stream>())).ReturnsAsync(new FileInfoDTO
            {
                Id = Guid.NewGuid(),
                Name = "Image",
                ContentType = "image/png",
                SizeKB = 1000
            });

            _facebookClient.Setup(f => f.GetImage(It.IsAny<string>()))
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new MultipartContent
                    {
                        Headers = { ContentType = MediaTypeHeaderValue.Parse("image/png")
                        }
                    },
                });
            AttachmentData attachments = new AttachmentData
            {
                Subattachments = null,
                Media = new Media
                {
                    Image = new Image()
                }
            };
            //act
            var list = await _fbPostCreator.GetPostImages(attachments, "123456789");

            //assert
            Assert.Equal(1, list.Count);
        }

        [Fact]
        public async Task GetPostImages_returns_list_with_three_images_if_subattachments_contain_three_images()
        {
            //arrange
            _fileService.Setup(f => f.CreateAsync(It.IsAny<CreateFileRequest>(), It.IsAny<Stream>())).ReturnsAsync(new FileInfoDTO
            {
                Id = Guid.NewGuid(),
                Name = "Image",
                ContentType = "image/jpeg",
                SizeKB = 1000
            });

            _facebookClient.Setup(f => f.GetImage(It.IsAny<string>()))
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new MultipartContent
                    {
                        Headers = { ContentType = MediaTypeHeaderValue.Parse("image/jpeg")
                        }
                    },
                });
            AttachmentData attachments = new AttachmentData
            {
                Subattachments = new SubAttachments
                {
                    Data = new List<SubAttachmentData>
                    {
                        new SubAttachmentData
                        {
                           Media = new Media
                           {
                                Image = new Image()
                           }
                        },
                        new SubAttachmentData
                        {
                            Media = new Media
                            {
                                Image = new Image()
                            }
                        },
                        new SubAttachmentData
                        {
                           Media = new Media
                           {
                                Image = new Image()
                           }
                        }
                    }
                }

            };
            //act
            var list = await _fbPostCreator.GetPostImages(attachments, It.IsAny<string>());

            //assert
            Assert.Equal(3, list.Count);
        }
    }
}
