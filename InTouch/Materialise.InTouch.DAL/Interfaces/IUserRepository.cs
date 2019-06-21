using Materialise.InTouch.DAL.Entities;
using System.Threading.Tasks;

namespace Materialise.InTouch.DAL.Interfaces
{
    public interface IUserRepository : IRepository<User, int>, IUserPaging<User>
    {
        Task<User> GetByEmailAsync(string roleName);
    }
}
