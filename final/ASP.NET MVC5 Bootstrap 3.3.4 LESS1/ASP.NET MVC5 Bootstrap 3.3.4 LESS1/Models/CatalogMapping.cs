using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.Models
{
    public class CatalogMapping
    {
        
           
            public string Id { get; set; }
           
            public string CatalogId { get; set; }
           
            public string ProductId { get; set; }
            
            public string CategoryId { get; set; }
         
            public string isFeatured { get; set; }
         
            public string FeaturedDisplayOrder { get; set; }
          
            public string isHomeProduct { get; set; }
        
            public string HomeProductDisplayOrder { get; set; }
       
            public string isActive { get; set; }
        
    }
}