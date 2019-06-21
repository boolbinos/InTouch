using Materialise.InTouch.BLL.ModelsDTO;
using System.Threading.Tasks;

namespace Materialise.InTouch.BLL.Interfaces
{
    public interface IUserContext
    {
        UserDTO CurrentUser { get; }
    }
}
