using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.BLL.Services.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            :base(message)
        {
        }
    }
}
