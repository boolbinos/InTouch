using System;
using System.Collections.Generic;
using System.Text;
using Materialise.InTouch.DAL.Interfaces;
using Materialise.InTouch.DAL.Repositories;
using Materialise.InTouch.DAL.Context;
using Materialise.InTouch.DAL.Entities;
using System.Threading.Tasks;

namespace Materialise.InTouch.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private InTouchContext db;
        private PostRepository PostRepository;
        private UserRepository UserRepository;
        private FileRepository FileRepository;
        private RoleRepository RoleRepository;
        private CommentRepository CommentRepository;
        private LikeRepository LikeRepository;

        public EFUnitOfWork(InTouchContext db)
        {
            this.db = db;
        }
        public IPostRepository Posts
        {
            get
            {
                if (PostRepository == null)
                    PostRepository = new PostRepository(db);
                return PostRepository;
            }
        }
        public IUserRepository Users
        {
            get
            {
                if (UserRepository == null)
                    UserRepository = new UserRepository(db);
                return UserRepository;
            }
        }

        public IRepository<FileInfo, Guid> Files
        {
            get
            {
                if (FileRepository == null)
                    FileRepository = new FileRepository(db);
                return FileRepository;
            }
        }

        public IRoleRepository<Role, int> Roles
        {
            get
            {
                if (RoleRepository == null)
                    RoleRepository = new RoleRepository(db);
                return RoleRepository;
            }
        }

        public ICommentRepository Comments
        {
            get
            {
                if (CommentRepository == null)
                    CommentRepository = new CommentRepository(db);
                return CommentRepository;
            }
        }


        public ILikeRepository UsersLikes
        {
            get
            {
                if (LikeRepository == null)
                    LikeRepository = new LikeRepository(db);
                return LikeRepository;
            }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }



        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
