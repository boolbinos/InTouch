using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Materialise.InTouch.WebSite.ViewModel
{
    public class CommentEmailModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public override bool Equals(object obj)
        {
            return Email == ((CommentEmailModel)obj).Email;
        }
        public override int GetHashCode()
        {
            return Email.GetHashCode();
        }
    }
}
