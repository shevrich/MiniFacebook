using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook.Data;

namespace FB050317.Models
{
    public class ImagesVM
    {
        public Image Image { get; set; }
        public User User { get; set; }
        public IEnumerable<Image> Likes { get; set; }
        public int UserLikes { get; set; }
    }
}