using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Materialise.InTouch.BLL.ModelsDTO
{
    public class InputPageData
    {
        [Range(1,9999)]
        public int PageNum { get; set; }

        [Range(1,99)]
        public int PageSize { get; set; }
        public bool JustMyPosts { get; set; }
        public string SearchStr { get; set; }    
    }
}
