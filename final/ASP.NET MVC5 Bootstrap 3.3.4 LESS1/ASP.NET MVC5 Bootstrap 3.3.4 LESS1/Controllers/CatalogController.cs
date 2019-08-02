using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using PagedList;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using NLog;

namespace ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.Controllers
{
    public class CatalogController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private CatalogContext db = new CatalogContext();

        public ActionResult Index(int? page)
        {
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    logger.Info("Catalog Index");

                    WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;



                    client.BaseAddress = new Uri
            ("http://localhost:65126/");
                    MediaTypeWithQualityHeaderValue contentType =
            new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    HttpResponseMessage response = client.GetAsync
            ("/api/catalog/").Result;
                    string stringData = response.Content.
            ReadAsStringAsync().Result;

                    List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Catalog> products = JsonConvert.DeserializeObject<List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Catalog>>(stringData);

                    return View(products.ToPagedList(page ?? 1, 8));
                }
                catch (Exception e)
                {
                    logger.Error(e, "Error in Catalog Index");
                    return View();
                }
            }

        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Id")]ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.Models.Catalog catalog)
        {
            catalog.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
               // ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client client = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
                //client.InsertCatalog(catalog);
                using (HttpClient client = new HttpClient())
                {
                    try { 
                        logger.Info("Catalog Create");
                    WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
                    client.BaseAddress = new Uri
            ("http://localhost:65126/");
                    MediaTypeWithQualityHeaderValue contentType =
            new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    var myContent = JsonConvert.SerializeObject(catalog);
                    string uri = client.BaseAddress + "api/catalog/InsertCatalog";
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.PostAsync(uri,byteContent ).Result;
                   // var result = client.PostAsync(string.Format("api/catalog/InsertCatalog?Id={0}&Name={1}&Description={2}&EnableAllProduct={3}&EnableAutoSync={4}&isActive={5}", catalog.Id, catalog.Name, catalog.Description, catalog.EnableAllProduct,catalog.EnableAutoSync,catalog.IsActive), byteContent).Result;
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, "Error in Catalog Create");
                    }

                }
                return RedirectToAction("Index");
            }

            return View(catalog);
        }

        

        public virtual FileResult buttonexport()
        {
            try { 
                logger.Info("Catalog Button Export");
            string strDelimiter = ",";

            ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client client = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Catalog> products = client.SelectCatalog().ToList();
             StringBuilder sb = new StringBuilder();           
                            sb.Append("Id" + strDelimiter);
                sb.Append("Name" + strDelimiter);
                sb.Append("Description" + strDelimiter);
                sb.Append("EnableAllProduct" + strDelimiter);
                sb.Append("EnableAutoSync" + strDelimiter);
                sb.Append("IsActive" + strDelimiter);
                sb.Append("\r\n");

                foreach (ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Catalog product in products)
                {
                    sb.Append(product.Id.ToString() + strDelimiter);

                    sb.Append(product.Name.ToString() + strDelimiter);
                    sb.Append(product.Description.ToString() + strDelimiter);
                    sb.Append(product.EnableAllProduct.ToString() + strDelimiter);
                    sb.Append(product.EnableAutoSync.ToString() + strDelimiter);
                    sb.Append(product.IsActive.ToString() + strDelimiter);
                    sb.Append("\r\n");

                }
            

            string strFileName = strDelimiter == "," ? "CatalogData.csv" : "CatalogData.txt";
            string filename = @Server.MapPath("~/App_Data/" + strFileName);
            if (System.IO.File.Exists(filename))
                System.IO.File.Delete(filename);
            StreamWriter file = new StreamWriter(filename);
            file.WriteLine(sb.ToString());
            file.Close();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Catalog Index");
            }
            byte[] fileBytes;
            string fileName;

            
            fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/CatalogData.csv"));
            fileName = "CatalogDetails.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
    }
}
