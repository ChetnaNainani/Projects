using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.Models
{
  public   class Product
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public int VendorProductId { get; set; }

        public string VendorProductSKU { get; set; }

        public int VendorCategoryId { get; set; }
    }
}
