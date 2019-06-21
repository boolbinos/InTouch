using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint;
using Materialise.InTouch.BLL.Services;
using Materialise.InTouch.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.Tests.Materialise.InTouch.BLL.Tests
{
    public class SharePointPostServiceTest
    {
        private readonly Mock<ILogger<ExternalPostService>> _logger;
        private readonly ExternalPostService _externalPostService;
        private readonly SharePointPostCreator _fbPostCreator;

        private readonly Mock<IExternalPostProviderFactory> _providerFactoryMock;
        private readonly Mock<IPostService> _postService;
        private readonly Mock<ISharePointClient> _facebookClient;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public SharePointPostServiceTest()
        {
                
        }
    }
}
