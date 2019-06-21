using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsDeleted { get; set; }

        public int RoleId { get; set; } = 1; // User

        public byte[] Avatar { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Role Role { get; set; }

        public ICollection<PostLike> PostsLike { get; set; } = new List<PostLike>();

        public bool IsDefaultUser()
        {
            return this.Id == 1;
        }

        public bool IsModerator()
        {
            return this.RoleId == Role.ModeratorRoleId;
        }
    }
}
