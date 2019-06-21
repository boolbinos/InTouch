using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public byte[] Avatar { get; set; }

    }
}
