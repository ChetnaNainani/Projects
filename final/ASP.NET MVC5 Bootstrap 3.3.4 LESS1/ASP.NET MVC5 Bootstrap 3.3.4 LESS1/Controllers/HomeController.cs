using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NLog;

namespace ASP.NET_MVC5_Bootstrap3_3_1_LESS.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        HttpPostedFileBase excelfile;
        
        public static int rowCount;

        //private ProductContext db = new ProductContext();

        
        [HttpPost]
        public ActionResult UploadFiles()
        {
            try
            {
                logger.Info("Home Upload Files");

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {

                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    excelfile = files[i];

                }
                    string path = Server.MapPath("~/Content/" + excelfile.FileName);
                    string path1 = Server.MapPath("~/App_Data/data1.xslx");
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    if (excelfile.FileName.EndsWith("xls"))
                    {
                        FileInfo f = new FileInfo(path);
                        f.MoveTo(Path.ChangeExtension(path, ".xlsx"));
                        string extension = Path.GetExtension(path);
                        path.Replace(extension, "xlsx");

                    }
                    else
                    {
                        excelfile.SaveAs(path);
                    }

                    FileInfo existingFile = new FileInfo(path);
                    DataTable dt1 = new DataTable("Products");
                    DataTable dt = new DataTable("Product Details");
                    dt1.Columns.Add("Id", typeof(string));
                    dt1.Columns.Add("ProductName", typeof(string));
                    dt1.Columns.Add("VendorProductId", typeof(string));
                    dt1.Columns.Add("VendorProductSKU", typeof(string));
                    dt1.Columns.Add("VendorCategoryId", typeof(string));
                    dt1.Columns.Add("ErrorDescription", typeof(string));
                    DataSet ds = new DataSet("ProductSet");
                    int  VendorProductId = 0, VendorCategoryId = 0;
                    string Id,ProductName, VendorProductSKU;
                    string VendorProductId1 , VendorCategoryId1 ;
                    using (ExcelPackage package = new ExcelPackage(existingFile))
                    {
                        //get the first worksheet in the workbook
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                        //List<Product> errorProducts = new List<Product>();
                        List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product> listProducts = new List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product>();
                        rowCount = workSheet.Dimension.End.Row;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product p = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product();

                            if (workSheet.Cells[row, 1].Value != null && workSheet.Cells[row, 2].Value != null && workSheet.Cells[row, 3].Value != null && workSheet.Cells[row, 4].Value != null && workSheet.Cells[row, 5].Value != null)
                         
                            {
                                p.Id = (workSheet.Cells[row, 1].Value.ToString() ?? null).ToString();
                                p.ProductName = (workSheet.Cells[row, 2].Value.ToString() ?? null).ToString();
                                p.VendorProductId = int.Parse((workSheet.Cells[row, 3].Value.ToString() ?? null).ToString());
                                p.VendorProductSKU = (workSheet.Cells[row, 4].Value.ToString() ?? null).ToString();
                                p.VendorCategoryId = int.Parse((workSheet.Cells[row, 5].Value.ToString() ?? null).ToString());


                                listProducts.Add(p);
                                Id = p.Id;
                                ProductName = p.ProductName;
                                VendorProductId = p.VendorProductId;
                                VendorProductSKU = p.VendorProductSKU;
                                VendorCategoryId = p.VendorCategoryId;
                                using (HttpClient client = new HttpClient())
                                {
                                    WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
                                    client.BaseAddress = new Uri
                            ("http://localhost:65126/");
                                    MediaTypeWithQualityHeaderValue contentType =
                            new MediaTypeWithQualityHeaderValue("application/json");
                                    client.DefaultRequestHeaders.Accept.Add(contentType);
                                    var myContent = JsonConvert.SerializeObject(p);
                                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                                    var byteContent = new ByteArrayContent(buffer);
                                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                    var result = client.PostAsync(string.Format("api/values/InsertProduct?Id={0}&ProductName={1}&VendorProductId={2}&VendorProductSKU={3}&VendorCategoryId={4}", Id, ProductName, VendorProductId, VendorProductSKU, VendorCategoryId),byteContent).Result;
                                 //   HttpResponseMessage response = client.PostAsync
                            //(string.Format("api/values/InsertProduct?Id={0}&ProductName={1}&VendorProductId={2}&VendorProductSKU={3}&VendorCategoryId={4}", Id, ProductName, VendorProductId, VendorProductSKU, VendorCategoryId), new StringContent(contents, Encoding.UTF8, "application/json")).Result;
                                    // var response = await client.GetAsync(string.format("api/products/id={0}&type={1}", param.Id.Value, param.Id.Type));
                                   // string stringData = response.Content.
                           // ReadAsStringAsync().Result;
                                   // List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product> products = JsonConvert.DeserializeObject<List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product>>(stringData);


                                    
                                }
                            }
                            else
                            {


                                string error = String.Empty;
                                if (workSheet.Cells[row, 1].Value == null)
                                {
                                    Id = String.Empty;
                                    error += " Id is Null";
                                }
                                else Id = (workSheet.Cells[row, 1].Value.ToString());
                                if (workSheet.Cells[row, 2].Value == null)
                                {
                                    ProductName = String.Empty;
                                    error += " Product Name is Null";
                                }
                                else ProductName = workSheet.Cells[row, 2].Value.ToString();
                                if (workSheet.Cells[row, 3].Value == null)
                                {
                                    VendorProductId1 = String.Empty;
                                    error += " Vendor Product Id is Null";
                                }
                                else VendorProductId1 = workSheet.Cells[row, 3].Value.ToString();
                                if (workSheet.Cells[row, 4].Value == null)
                                {
                                    VendorProductSKU = String.Empty;
                                    error += " Vendor Product SKU is Null";
                                }
                                else VendorProductSKU = workSheet.Cells[row, 4].Value.ToString();
                                if (workSheet.Cells[row, 5].Value == null)
                                {
                                    VendorCategoryId1 = String.Empty;
                                    error += " Vendor Category is Null";
                                }
                                else VendorCategoryId1 = workSheet.Cells[row, 5].Value.ToString();
                                dt1.Rows.Add(Id, ProductName, VendorProductId1, VendorProductSKU, VendorCategoryId1, error);



                            }
                        }

                        ds.Tables.Add(dt1);
                        using (ExcelPackage objExcelPackage = new ExcelPackage())
                        {
                            foreach (DataTable dtSrc in ds.Tables)
                            {
                                //Create the worksheet    
                                ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(dtSrc.TableName);
                                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                                objWorksheet.Cells["A1"].LoadFromDataTable(dtSrc, true);
                                objWorksheet.Cells.Style.Font.SetFromFont(new Font("Calibri", 10));
                                objWorksheet.Cells.AutoFitColumns();
                                //Format the header    
                                using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                                {
                                    objRange.Style.Font.Bold = true;
                                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;

                                }
                            }

                            string tempFile = Path.GetTempPath();
                            
                            if (System.IO.File.Exists(path1))
                                System.IO.File.Delete(path1);

                            //Create excel file on physical disk    
                            using (FileStream objFileStrm = System.IO.File.Create(Server.MapPath("~/App_Data/data1.xlsx"))) { }

                            //Write content to excel file    
                            System.IO.File.WriteAllBytes(Server.MapPath("~/App_Data/data1.xlsx"), objExcelPackage.GetAsByteArray());


                            //errorProducts.Add(p);



                            string fileName = Server.MapPath("~/App_Data/data1.txt");
                            using (StreamWriter fs = System.IO.File.CreateText(fileName))
                            {

                                foreach (var p1 in listProducts)
                                {
                                    fs.WriteLine("Id: " + p1.Id + " ");
                                    fs.WriteLine("ProductName: " + p1.ProductName + " ");
                                    fs.WriteLine("VendorProductId: " + p1.VendorProductId + " ");
                                    fs.WriteLine("VendorProductSKU: " + p1.VendorProductSKU + " ");
                                    fs.WriteLine("VendorCategoryId: " + p1.VendorCategoryId + " ");

                                }

                            }
                        }

                    }

                
        
                return Json("number of rows in the file: " + rowCount.ToString());


            }
            else
            {
                return Json("No files selected.");
            }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Home Upload Files");
                return View();
            }

        }
        public virtual FileResult Download()
        {
             try { 
                logger.Info("Home Download");
            byte[] fileBytes;
            string fileName;


            fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/data1.xlsx"));
            fileName = "ErrorReport.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

             }
             catch (Exception e)
             {
                 logger.Error(e, "Error in Home Download");
                 return File("", "", "");
             }

        }


        public ActionResult Index(int? page, string search)
        {
            using (HttpClient client = new HttpClient())
            { try { 
                    logger.Info("Home Index");
                WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
                client.BaseAddress = new Uri
        ("http://localhost:65126/");
                MediaTypeWithQualityHeaderValue contentType =
        new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync
        (string.Format("api/values/Get?search={0}", search)).Result;
               // var response = await client.GetAsync(string.format("api/products/id={0}&type={1}", param.Id.Value, param.Id.Type));
                string stringData = response.Content.
        ReadAsStringAsync().Result;
                List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product> products = JsonConvert.DeserializeObject<List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product>>(stringData);
                if (search == null)
                    return View(products.ToPagedList(page ?? 1, 8));
               
                return View(products.ToPagedList(page ?? 1, 8));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Home Index");
                return View();
            }
            }
            
        }
        [HttpPost]
        public ActionResult ReadFiles()
        {
             try { 
                logger.Info("Home ReadFiles");
            int rowCount1 = 0;
            if (Request.Files.Count > 0)
            {

                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    excelfile = files[i];

                    var excel = new ExcelPackage(excelfile.InputStream);
                    var ws = excel.Workbook.Worksheets.First();
                    rowCount1 = ws.Dimension.End.Row;


                }
                return Json(rowCount1.ToString());
            }
            else
            {
                return Json("error");
            }
             }
             catch (Exception e)
             {
                 logger.Error(e, "Error in Home ReadFiles");
                 return View();
             }

        }

        public virtual FileResult buttonexport()
        {
             try { 
                logger.Info("Home Button Export");
            string strDelimiter = ",";
            string search = null;
            ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client client = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product> products = client.SelectProduct(search).ToList();
             StringBuilder sb = new StringBuilder();           
                            sb.Append("Id" + strDelimiter);
                sb.Append("ProductName" + strDelimiter);
                sb.Append("VendorProductId" + strDelimiter);
                sb.Append("VendorProductSKU" + strDelimiter);
                sb.Append("VendorCategoryId" + strDelimiter);

                sb.Append("\r\n");
                   
                foreach (ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Product product in products)
                {
                    sb.Append(product.Id.ToString() + strDelimiter);
                    sb.Append(product.ProductName.ToString() + strDelimiter);
                    sb.Append(product.VendorProductId.ToString() + strDelimiter);
                    sb.Append(product.VendorProductSKU.ToString() + strDelimiter);
                    sb.Append(product.VendorCategoryId.ToString() + strDelimiter);
                    sb.Append("\r\n");

                }
            

            string strFileName = strDelimiter == "," ? "ProductData.csv" : "ProductData.txt";
            string filename = @Server.MapPath("~/App_Data/" + strFileName);
            if (System.IO.File.Exists(filename))
                System.IO.File.Delete(filename);
            StreamWriter file = new StreamWriter(filename);
            file.WriteLine(sb.ToString());
            file.Close();
             }
             catch (Exception e)
             {
                 logger.Error(e, "Error in Home Button Export");

             }
            byte[] fileBytes;
            string fileName;


            fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/ProductData.csv"));
            fileName = "ProductDetails.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }


    }

}