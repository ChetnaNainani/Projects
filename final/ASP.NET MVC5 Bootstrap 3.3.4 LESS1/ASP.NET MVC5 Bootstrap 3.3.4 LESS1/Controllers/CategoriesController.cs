using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Excel = Microsoft.Office.Interop.Excel;
using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;
using OfficeOpenXml;

using System.Data;
using System.Drawing;
using OfficeOpenXml.Style;
using PagedList.Mvc;
using PagedList;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NLog;

namespace ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.Controllers
{
    public class CategoriesController : Controller
    {
        // private emp1Entities7 db = new emp1Entities7();

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index(int? page)
        {
            // ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client client = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            //List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Category> products = client.SelectCategory().ToList();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    logger.Info("Category Index");
                    WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
                    client.BaseAddress = new Uri
                        ("http://localhost:65126/");
                    MediaTypeWithQualityHeaderValue contentType =
                         new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    HttpResponseMessage response = client.GetAsync
                    ("/api/category/").Result;
                    string stringData = response.Content.
                        ReadAsStringAsync().Result;
                    List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Category> products = JsonConvert.DeserializeObject<List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Category>>(stringData);

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
        public ActionResult Create([Bind(Exclude = "Id")] ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.Models.Category category)
        {
            category.Id = Guid.NewGuid().ToString();
            category.MetaTitle = "";
            category.MetaKeywords = "";
            category.MetaDescription = "";
            category.ImageUrl = "";
            var client = new HttpClient();
             if (ModelState.IsValid)
            {
                // ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client client = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
                // client.InsertCategory(category);

                try
                {
                    logger.Info("Category Create");
                    WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                   
                   
                        client.BaseAddress = new Uri("http://localhost:65126/");
                        MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                        client.DefaultRequestHeaders.Accept.Add(contentType);
                        var myContent = JsonConvert.SerializeObject(category);
                        string uri = client.BaseAddress + "api/category/InsertCategory";
                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        var httpContent = new StringContent(myContent);
                        try
                        {
                          //  var content = new FormUrlEncodedContent(httpContent.ToString);
                          //  var content = new StringContent(category.ToString(), Encoding.UTF8, "application/json");
                            MemoryStream stream = new MemoryStream();
                            JsonConvert.SerializeObject(category.GetType());
                           // json_functio.serianslizer(category, category.GetType(), ref stream);
                            HttpContent content = new StreamContent (stream);
                          //  HttpContent content1 = new StringContent(content1.ToString, Encoding.UTF8, "application/json"));
                           // WebApi.insertCategory(category, category.GetType(), ref stream);
                          //  HttpResponseMessage response = client.PostAsync("user/create", content).Result;
                           
                          //  HttpContent content2 = new StringContent(content1, Encoding.UTF8, "application/json")).Result;
                            HttpResponseMessage response = client.PostAsync(uri, byteContent).Result;

                           // HttpResponseMessage response = client.PutAsJsonAsync(uri, JsonConvert.SerializeObject(category)).Result;  
                            
                        }
                        catch(Exception e)
                        {
                            logger.Error(e, "Error in Category Create");
                        }
                    }
                    // var result = client.PostAsync(string.Format("api/category/InsertCategory?Id={0}&Name={1}&Code={2}&DisplayName={3}&ImageUrl={4}&DisplayPriority={5}&MetaTitle={6}&MetaKeywords={7}&MetaDescription={8}&isActive={9}", category.Id,category.Name,category.Code,category.DisplayName,category.ImageUrl,category.DisplayPriority,category.MetaTitle,category.MetaKeywords,category.MetaDescription, category.isActive), byteContent).Result;
                
                catch (Exception e)
                {
                    logger.Error(e, "Error in Category Create");

                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        public virtual FileResult buttonexport()
        {
            try
            {
                logger.Info("Button Export in Category");
                string strDelimiter = ",";

                ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client client = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
                List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Category> products = client.SelectCategory().ToList();
                StringBuilder sb = new StringBuilder();
                sb.Append("Id" + strDelimiter);
                sb.Append("Name" + strDelimiter);
                sb.Append("Code" + strDelimiter);
                sb.Append("DisplayName" + strDelimiter);
                sb.Append("ImageUrl" + strDelimiter);
                sb.Append("DisplayPriority" + strDelimiter);

                sb.Append("MetaTitle" + strDelimiter);
                sb.Append("MetaKeywords" + strDelimiter);
                sb.Append("MetaDescription" + strDelimiter);
                sb.Append("isActive");
                sb.Append("\r\n");

                foreach (ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Category product in products)
                {
                    sb.Append(product.Id.ToString() + strDelimiter);

                    sb.Append(product.Name.ToString() + strDelimiter);
                    sb.Append(product.Code.ToString() + strDelimiter);
                    sb.Append(product.DisplayName.ToString() + strDelimiter);
                    if (product.ImageUrl == null)
                        sb.Append(String.Empty + strDelimiter);
                    else
                        sb.Append(product.ImageUrl + strDelimiter);
                    sb.Append(product.DisplayPriority.ToString() + strDelimiter);

                    if (product.MetaTitle == null)
                        sb.Append(String.Empty + strDelimiter);
                    else
                        sb.Append(product.MetaTitle + strDelimiter);
                    if (product.MetaKeywords == null)
                        sb.Append(String.Empty + strDelimiter);
                    else
                        sb.Append(product.MetaKeywords + strDelimiter);
                    if (product.MetaDescription == null)
                        sb.Append(String.Empty + strDelimiter);
                    else
                        sb.Append(product.MetaDescription + strDelimiter);
                    sb.Append(product.isActive.ToString().ToUpper());

                    sb.Append("\r\n");

                }


                string strFileName = strDelimiter == "," ? "Data.csv" : "Data.txt";
                string filename = @Server.MapPath("~/App_Data/" + strFileName);
                if (System.IO.File.Exists(filename))
                    System.IO.File.Delete(filename);
                StreamWriter file = new StreamWriter(filename);
                file.WriteLine(sb.ToString());
                file.Close();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Category Button Export ");

            }
            byte[] fileBytes;
            string fileName;


            fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/Data.csv"));
            fileName = "Category.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
    }
}
