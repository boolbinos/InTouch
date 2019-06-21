using Materialise.InTouch.BLL.ModelsDTO;

namespace Materialise.InTouch.WebSite.Extensions
{
    public static class UserDTOExtension
    {
        public static bool IsModerator(this UserDTO user) => user.RoleId == Consts.Roles.ModeratorRoleId;

    }
}
