using Materialise.InTouch.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository Posts { get; }
        IUserRepository Users { get; }
        ICommentRepository Comments { get; }
        IRepository<FileInfo, Guid> Files{get;}
        IRoleRepository<Role, int> Roles { get; }
        ILikeRepository UsersLikes { get; }
        Task SaveAsync();
    }
}
