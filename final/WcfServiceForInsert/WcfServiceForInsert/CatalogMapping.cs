using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WcfServiceForInsert
{
    
    [Table("catalogmapping")]
    public class CatalogMapping
    {
       
        public string Id { get; set; }
       
        public string CatalogId { get; set; }
      
        public string ProductId { get; set; }
        
        public string CategoryId { get; set; }
     
        public bool isFeatured { get; set; }
       
        public string FeaturedDisplayOrder { get; set; }

        public bool isHomeProduct { get; set; }
        
        public string HomeProductDisplayOrder { get; set; }

        public bool isActive { get; set; }
    }
}
