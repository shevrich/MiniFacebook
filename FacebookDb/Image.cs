using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateUploaded { get; set; }
        public int Views { get; set; }
        public int LikesCount { get; set; }
    }
}
