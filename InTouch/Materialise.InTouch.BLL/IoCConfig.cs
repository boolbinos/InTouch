using Materialise.InTouch.BLL.Interfaces;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.Facebook;
using Materialise.InTouch.BLL.Providers.ExternalPostProviders.SharePoint;
using Materialise.InTouch.BLL.Services;
using Materialise.InTouch.BLL.Services.Storage;
using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Interfaces;
using Materialise.InTouch.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Materialise.InTouch.BLL
{
    public static class IoCConfig
    {
        public static void SetupDB(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<InTouchContext>(options =>
                options.UseSqlServer(connectionString));
        }

        public static void Setup(this IServiceCollection services)
        {
            
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            services.AddScoped<IExternalPostService, ExternalPostService>();
            services.AddTransient<IExternalPostProviderFactory, ExternalPostProviderFactory>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFacebookClient, FacebookClient>();
            services.AddTransient<ISharePointClient, SharePointClient>();
            services.AddTransient<IFacebookPostCreator, FacebookPostCreator>();
            services.AddTransient<ISharePointPostCreator, SharePointPostCreator>();
            services.AddTransient<IFullscreenBatchService, FullscreenBatchService>();
            services.AddTransient<ILikeService, LikeService>();

            services.AddTransient<IExternalPostProvider, FacebookPostProvider>();
            services.AddTransient<IExternalPostProvider, SharepointPostProvider>();
            services.AddTransient<IExternalPostProviderFactory, ExternalPostProviderFactory>();

            services.AddTransient<DatabaseInitializer>();
        }
    }
}
