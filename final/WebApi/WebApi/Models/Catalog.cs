using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
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