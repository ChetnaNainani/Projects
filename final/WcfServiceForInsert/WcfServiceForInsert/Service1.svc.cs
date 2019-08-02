using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web;
using Microsoft.SqlServer.Server;
using GiftcardServiceForInsert;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data.Entity;
using MySql.Data.EntityFramework;
using NLog;  
namespace WcfServiceForInsert
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.  
   [GlobalErrorBehaviorAttribute(typeof(GlobalErrorHandler))]
    public class Service1 : IService1
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        

        public   List<Product> SelectProduct(string search)
        {
            try
            {
                logger.Info("Select Product");
                GiftcardContext db = new GiftcardContext();
                if (search == null)
                    return db.Products.ToList();

                return db.Products.Where(x => x.ProductName.StartsWith(search)).ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Select Product");
                return new List<Product>();
            }
        }

        public void InsertProduct(Product product)
        {
            try
            {
                logger.Info("Insert Product");
            GiftcardContext db = new GiftcardContext();
            Product p = new Product();
            p.Id = product.Id;
            p.ProductName = product.ProductName;
            p.VendorProductId = product.VendorProductId;
            p.VendorProductSKU = product.VendorProductSKU;
            p.VendorCategoryId = product.VendorCategoryId;
            db.Products.Add(p);
            db.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Insert Product");
                throw new NotImplementedException();
            } 
        }


        public List<CatalogMapping> SelectCatalogMapping(string search)
        {
             try
            {
                logger.Info("Select Catalog Mapping");
            GiftcardContext db = new GiftcardContext();
            if (search == null)
                return db.CatalogMappings.ToList();

            return db.CatalogMappings.Where(x => x.CatalogId.StartsWith(search)).ToList();
            }
             catch (Exception e)
             {
                 logger.Error(e, "Error in Select Catalog Mapping");
                 return new List<CatalogMapping>();
             }
        }

        public void InsertCatalogMapping(CatalogMapping product)
        {

            try
            {
                logger.Info("Insert Catalog Mapping");
            GiftcardContext db = new GiftcardContext();
            
            
            db.CatalogMappings.Add(product);
            db.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Insert Catalog Mapping");
                throw new NotImplementedException();
            }
        }

        public List<Catalog> SelectCatalog()
        {
            try
            {
                logger.Info("Select Catalog");
            GiftcardContext db = new GiftcardContext();
            
                return db.Catalogs.ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Select Catalog ");
                return new List<Catalog>();
            }
            //return db.Catalogs.Where(x => x.CatalogId.StartsWith(search)).ToList();
        }

        public void InsertCatalog(Catalog product)
        {
            try
            {
                logger.Info("Insert Catalog");
            GiftcardContext db = new GiftcardContext();


            db.Catalogs.Add(product);
            db.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Insert Catalog ");
                throw new NotImplementedException();
            }
        }

        public List<Category> SelectCategory()
        {
            try
            {
                logger.Info("Select Category");
            GiftcardContext db = new GiftcardContext();
            //if (search == null)
                return db.Categories.ToList();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Select Category ");
                return new List<Category>();
            }
            //return db.CatalogMappings.Where(x => x.CatalogId.StartsWith(search)).ToList();
        }

        public void InsertCategory(Category product)
        {
          
            GiftcardContext db = new GiftcardContext();


            db.Categories.Add(product);
            db.SaveChanges();
           
        }
       



    }
}