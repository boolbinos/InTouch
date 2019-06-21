using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public static int UserRoleId { get; } = 1;
        public static int ModeratorRoleId { get; } = 2;
    }
}
