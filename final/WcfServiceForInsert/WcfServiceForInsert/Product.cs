using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WcfServiceForInsert
{
   
    [Table("product")]
    public class Product
    {
     
        [Column("Id")]
        public string Id { get; set; }
        
        [Column("ProductName")]
        public string ProductName { get; set; }
       
        [Column("VendorProductId")]
        public int VendorProductId { get; set; }
      
        [Column("VendorProductSKU")]
        public string VendorProductSKU { get; set; }
      
        [Column("VendorCategoryId")]
        public int VendorCategoryId { get; set; }
    }
}
