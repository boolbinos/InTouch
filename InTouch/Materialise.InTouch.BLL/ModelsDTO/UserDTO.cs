using System.Collections.Generic;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsDeleted { get; set; }

        public int RoleId { get; set; }

        public byte[] Avatar { get; set; }


        public ICollection<PostDTO> PostsDTO { get; set; }

        public RoleDTO RoleDTO { get; set; }
    }
}