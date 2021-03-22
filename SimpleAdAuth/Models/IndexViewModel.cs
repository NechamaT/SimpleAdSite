using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleAdAuth.Data;
namespace SimpleAdAuth.Models
{
    public class IndexViewModel
    {
        public List<Ad> Ads { get; set; }
        public User CurrentUser { get; set; }
    }
}
