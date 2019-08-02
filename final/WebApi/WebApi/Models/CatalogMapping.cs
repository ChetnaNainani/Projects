using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
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