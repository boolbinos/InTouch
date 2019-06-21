using Materialise.InTouch.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface IPostRepository:IRepository<Post, int>, IPostPaging<Post>
    {
    }
}
