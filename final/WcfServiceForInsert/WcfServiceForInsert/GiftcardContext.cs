using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WcfServiceForInsert;
using MySql.Data;
using System.Runtime.Serialization;
using MySql.Data.EntityFramework;
namespace GiftcardServiceForInsert
{
   
    [DataContract]
    

    public class GiftcardContext : DbContext
    {
      public GiftcardContext() : base("name=GiftcardContext") { this.Configuration.LazyLoadingEnabled = false; }
        [DataMember]
        public DbSet<Product> Products { get; set; }
        [DataMember]
        public DbSet<Catalog> Catalogs { get; set; }
        [DataMember]
        public DbSet<Category> Categories { get; set; }
        [DataMember]
        public DbSet<CatalogMapping> CatalogMappings { get; set; }
    }
}