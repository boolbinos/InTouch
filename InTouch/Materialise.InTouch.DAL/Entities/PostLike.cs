﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Materialise.InTouch.DAL.Entities
{
    public class PostLike
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
