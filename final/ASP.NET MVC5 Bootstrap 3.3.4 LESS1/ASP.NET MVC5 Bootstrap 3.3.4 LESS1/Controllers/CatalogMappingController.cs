using System;
using System.Collections.Generic;
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
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NLog;
namespace ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.Controllers
{
    public class CatalogMappingController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        HttpPostedFileBase excelfile;
        public static int rowCount;
        [HttpPost]
        public ActionResult UploadFiles()
        {
            try
            {
                logger.Info("Catalog Mapping Upload Files");

                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {

                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        excelfile = files[i];
                        //string fname;

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
                        DataTable dt1 = new DataTable("Catalog Product Mapping");
                        dt1.Columns.Add("Id", typeof(string));
                        dt1.Columns.Add("CatalogId", typeof(string));
                        dt1.Columns.Add("ProductId", typeof(string));
                        dt1.Columns.Add("CatgoryId", typeof(string));
                        dt1.Columns.Add("isFeatured", typeof(string));
                        dt1.Columns.Add("FeaturedDisplayOrder", typeof(string));
                        dt1.Columns.Add("isHomeProduct", typeof(string));
                        dt1.Columns.Add("HomeProductDisplayOrder", typeof(string));
                        dt1.Columns.Add("isActive", typeof(string));
                        dt1.Columns.Add("ErrorDescription", typeof(string));
                        DataSet ds = new DataSet("Catalog Product Mapping");
                        string Id, CatalogId, ProductId, CategoryId, FeaturedDisplayOrder, HomeProductDisplayOrder, isFeatured, isHomeProduct, isActive;
                        bool isFeatured1, isHomeProduct1, isActive1;
                        using (ExcelPackage package = new ExcelPackage(existingFile))
                        {
                            //get the first worksheet in the workbook
                            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                            //  List<CatalogProductMapping> errorProducts = new List<CatalogProductMapping>();
                            List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.CatalogMapping> listProducts = new List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.CatalogMapping>();
                            rowCount = workSheet.Dimension.End.Row;
                            for (int row = 2; row <= rowCount; row++)
                            {
                                ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.CatalogMapping p = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.CatalogMapping();

                                if (workSheet.Cells[row, 1].Value != null && workSheet.Cells[row, 2].Value != null && workSheet.Cells[row, 3].Value != null && workSheet.Cells[row, 4].Value != null && workSheet.Cells[row, 5].Value != null && workSheet.Cells[row, 6].Value != null && workSheet.Cells[row, 7].Value != null && workSheet.Cells[row, 8].Value != null && workSheet.Cells[row, 9].Value != null)
                                {
                                    p.Id = (workSheet.Cells[row, 1].Value.ToString() ?? null).ToString();
                                    p.CatalogId = (workSheet.Cells[row, 2].Value.ToString() ?? null).ToString();
                                    p.ProductId = (workSheet.Cells[row, 3].Value.ToString() ?? null).ToString();
                                    p.CategoryId = (workSheet.Cells[row, 4].Value.ToString() ?? null).ToString();
                                    p.isFeatured = (bool)(workSheet.Cells[row, 5].Value ?? null);
                                    p.FeaturedDisplayOrder = (workSheet.Cells[row, 6].Value.ToString() ?? null).ToString();
                                    p.isHomeProduct = (bool)(workSheet.Cells[row, 7].Value ?? null);
                                    p.HomeProductDisplayOrder = (workSheet.Cells[row, 8].Value.ToString() ?? null).ToString();
                                    p.isActive = (bool)(workSheet.Cells[row, 9].Value ?? null);

                                    listProducts.Add(p);
                                    //ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client client = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client();
                                    //client.InsertCatalogMapping(p);
                                    Id = p.Id;
                                    CatalogId = p.CatalogId;
                                    ProductId = p.ProductId;
                                    CategoryId = p.CategoryId;
                                    isFeatured1 = p.isFeatured;
                                    FeaturedDisplayOrder = p.FeaturedDisplayOrder;
                                    isHomeProduct1 = p.isHomeProduct;
                                    HomeProductDisplayOrder = p.HomeProductDisplayOrder;
                                    isActive1 = p.isActive;
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
                                        var result = client.PostAsync(string.Format("api/catalogmapping/InsertCatalogMapping?Id={0}&CatalogId={1}&ProductId={2}&CategoryId={3}&isFeatured={4}&FeaturedDisplayOrder={5}&isHomeProduct={6}&HomeProductDisplayOrder={7}&isActive={8}", Id, CatalogId, ProductId, CategoryId, isFeatured1, FeaturedDisplayOrder, isHomeProduct1, HomeProductDisplayOrder, isActive1), byteContent).Result;


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
                                    else Id = workSheet.Cells[row, 1].Value.ToString();
                                    if (workSheet.Cells[row, 2].Value == null)
                                    {
                                        CatalogId = String.Empty;
                                        error += " CatalogId is Null";
                                    }
                                    else CatalogId = workSheet.Cells[row, 2].Value.ToString();
                                    if (workSheet.Cells[row, 3].Value == null)
                                    {
                                        ProductId = String.Empty;
                                        error += " ProductId is Null";
                                    }
                                    else ProductId = workSheet.Cells[row, 3].Value.ToString();
                                    if (workSheet.Cells[row, 4].Value == null)
                                    {
                                        CategoryId = String.Empty;
                                        error += " CategoryId is Null";
                                    }
                                    else CategoryId = workSheet.Cells[row, 4].Value.ToString();
                                    if (workSheet.Cells[row, 5].Value == null)
                                    {
                                        isFeatured = String.Empty;
                                        error += " isFeatured is Null";
                                    }
                                    else isFeatured = workSheet.Cells[row, 5].Value.ToString();
                                    if (workSheet.Cells[row, 6].Value == null)
                                    {
                                        FeaturedDisplayOrder = String.Empty;
                                        error += " FeaturedDisplayOrder is Null";
                                    }
                                    else FeaturedDisplayOrder = workSheet.Cells[row, 6].Value.ToString();
                                    if (workSheet.Cells[row, 7].Value == null)
                                    {
                                        isHomeProduct = String.Empty;
                                        error += " isHomeProduct is Null";
                                    }
                                    else isHomeProduct = workSheet.Cells[row, 6].Value.ToString();
                                    if (workSheet.Cells[row, 7].Value == null)
                                    {
                                        HomeProductDisplayOrder = String.Empty;
                                        error += " HomeProductDisplayOrder is Null";
                                    }
                                    else HomeProductDisplayOrder = workSheet.Cells[row, 6].Value.ToString();
                                    if (workSheet.Cells[row, 9].Value == null)
                                    {
                                        isActive = String.Empty;
                                        error += " isActive is Null";
                                    }
                                    else isActive = workSheet.Cells[row, 6].Value.ToString();
                                    dt1.Rows.Add(Id, CatalogId, ProductId, CategoryId, isFeatured, FeaturedDisplayOrder, isHomeProduct, HomeProductDisplayOrder, isActive, error);



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
                                // StreamWriter downloadWriter =new StreamWriter(Path.GetTempPath());
                                //Write it back to the client    
                                if (System.IO.File.Exists(path1))
                                    System.IO.File.Delete(path1);

                                //Create excel file on physical disk    
                                using (FileStream objFileStrm = System.IO.File.Create(Server.MapPath("~/App_Data/data1.xlsx"))) { }

                                System.IO.File.WriteAllBytes(Server.MapPath("~/App_Data/data1.xlsx"), objExcelPackage.GetAsByteArray());


                                string fileName = Server.MapPath("~/App_Data/data.txt");
                                using (StreamWriter fs = System.IO.File.CreateText(fileName))
                                {

                                    foreach (var p1 in listProducts)
                                    {

                                        fs.WriteLine("Id: " + p1.Id + " ");

                                        fs.WriteLine("CatalogId: " + p1.CatalogId + " ");
                                        fs.WriteLine("ProductId: " + p1.ProductId + " ");
                                        fs.WriteLine("CategoryId: " + p1.CategoryId + " ");
                                        fs.WriteLine("isFeatured: " + p1.isFeatured + " ");
                                        fs.WriteLine("FeaturedDisplayOrder: " + p1.FeaturedDisplayOrder + " ");
                                        fs.WriteLine("isHomeProduct: " + p1.isHomeProduct + " ");
                                        fs.WriteLine("HomeProductDisplayOrder: " + p1.HomeProductDisplayOrder + " ");
                                        fs.WriteLine("isActive: " + p1.isActive + " ");


                                    }


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
                logger.Error(e, "Error in Catalog Mapping Upload Files");
                return View();
            }

        }
            
        public virtual FileResult Download()
        {
            try { 
                logger.Info("Catalog Mapping Download");
            byte[] fileBytes;
            string fileName;


            fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/data1.xlsx"));
            fileName = "ErrorReport.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Catalog Mapping Download");
                return File("","","");
            }


        }


        public ActionResult Index(int? page, string search)
        {
            using (HttpClient client = new HttpClient())
            {
                try { 
                    logger.Info("CatalogMapping Index");
                WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
                client.BaseAddress = new Uri
        ("http://localhost:65126/");
                MediaTypeWithQualityHeaderValue contentType =
        new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync
        (string.Format("api/catalogmapping/Get?search={0}", search)).Result;
                // var response = await client.GetAsync(string.format("api/products/id={0}&type={1}", param.Id.Value, param.Id.Type));
                string stringData = response.Content.
        ReadAsStringAsync().Result;
                List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.CatalogMapping> products = JsonConvert.DeserializeObject<List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.CatalogMapping>>(stringData);
                if (search == null)
                    return View(products.ToPagedList(page ?? 1, 8));

                return View(products.ToPagedList(page ?? 1, 8));
                }
                catch (Exception e)
                {
                    logger.Error(e, "Error in Catalog Mapping Index");
                    return View();
                }
            }

        }
        [HttpPost]
        public ActionResult ReadFiles()
        {
            try { 
                logger.Info("Catalog Mapping ReadFiles");
            int rowCount1 = 0;
            if (Request.Files.Count > 0)
            {
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
                logger.Error(e, "Error in Catalog Mapping ReadFiles");
                return View();
            }
        }

        public virtual FileResult buttonexport()
        {
            try { 
                logger.Info("Catalog Mapping Button Export");
            string strDelimiter = ",";
            string search = null;
            ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client client = new ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            List<ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.CatalogMapping> products = client.SelectCatalogMapping(search).ToList();
             StringBuilder sb = new StringBuilder();           
                            sb.Append("Id" + strDelimiter);
                sb.Append("CatalogId" + strDelimiter);
                sb.Append("ProductId" + strDelimiter);
                sb.Append("CategoryId" + strDelimiter);
                sb.Append("isFeatured" + strDelimiter);
                sb.Append("FeaturedDisplayOrder" + strDelimiter);
                sb.Append("isHomeProduct" + strDelimiter);
                sb.Append("HomeProductDisplayOrder" + strDelimiter);

                sb.Append("isActive");
                sb.Append("\r\n");

                foreach (ASP.NET_MVC5_Bootstrap_3._3._4_LESS1.ServiceReference1.CatalogMapping product in products)
                {
                    sb.Append(product.Id.ToString() + strDelimiter);

                    sb.Append(product.CatalogId.ToString() + strDelimiter);
                    sb.Append(product.ProductId.ToString() + strDelimiter);
                    sb.Append(product.CategoryId.ToString() + strDelimiter);
                    sb.Append(product.isFeatured.ToString() + strDelimiter);
                    sb.Append(product.FeaturedDisplayOrder.ToString() + strDelimiter);
                    sb.Append(product.isHomeProduct.ToString() + strDelimiter);
                    sb.Append(product.HomeProductDisplayOrder.ToString() + strDelimiter);
                    sb.Append(product.isActive.ToString());
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
                logger.Error(e, "Error in Catalog Mapping Button Export");
                
            }
            byte[] fileBytes;
            string fileName;


            fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/Data.csv"));
            fileName = "CatalogMappingDetails.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

    }
}