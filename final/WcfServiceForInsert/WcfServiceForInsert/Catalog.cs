using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WcfServiceForInsert
{
   [Table("catalog")]
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
