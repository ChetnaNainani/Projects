using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.Models
{
    public class Catalog
    {
        public string Id { get; set; }
     
        public string Name { get; set; }
      
        public string Description { get; set; }
       
        public bool EnableAllProduct { get; set; }
      
        public bool EnableAutoSync { get; set; }
       
        public bool IsActive { get; set; }
    }
}