using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;
using OfficeOpenXml;
using System.Data;
using System.Drawing;
using OfficeOpenXml.Style;
using PagedList;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        HttpPostedFileBase excelfile;

        public static int rowCount;

        //private ProductContext db = new ProductContext();



        public ActionResult UploadFiles()
        {
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
                dt1.Columns.Add("VendorProductId", typeof(int));
                dt1.Columns.Add("VendorProductSKU", typeof(string));
                dt1.Columns.Add("VendorCategoryId", typeof(int));
                dt1.Columns.Add("ErrorDescription", typeof(string));
                DataSet ds = new DataSet("ProductSet");
                int VendorProductId = 0, VendorCategoryId = 0;
                string Id, ProductName, VendorProductSKU;
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    //get the first worksheet in the workbook
                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                    //List<Product> errorProducts = new List<Product>();
                    List<WebApi.ServiceReference1.Product> listProducts = new List<WebApi.ServiceReference1.Product>();
                    rowCount = workSheet.Dimension.End.Row;
                    for (int row = 2; row < rowCount; row++)
                    {
                        WebApi.ServiceReference1.Product p = new WebApi.ServiceReference1.Product();

                        if (workSheet.Cells[row, 1].Value != null && workSheet.Cells[row, 2].Value != null && workSheet.Cells[row, 3].Value != null && workSheet.Cells[row, 4].Value != null && workSheet.Cells[row, 5].Value != null)
                        {
                            p.Id = (workSheet.Cells[row, 1].Value.ToString() ?? null).ToString();
                            p.ProductName = (workSheet.Cells[row, 2].Value.ToString() ?? null).ToString();
                            p.VendorProductId = int.Parse((workSheet.Cells[row, 3].Value.ToString() ?? null).ToString());
                            p.VendorProductSKU = (workSheet.Cells[row, 4].Value.ToString() ?? null).ToString();
                            p.VendorCategoryId = int.Parse((workSheet.Cells[row, 5].Value.ToString() ?? null).ToString());


                            listProducts.Add(p);
                            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client();
                            client.InsertProduct(p);

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
                                VendorProductId = 0;
                                error += " Vendor Product Id is Null";
                            }
                            else VendorProductId = (int)(workSheet.Cells[row, 3].Value);
                            if (workSheet.Cells[row, 4].Value == null)
                            {
                                VendorProductSKU = String.Empty;
                                error += " Vendor Product SKU is Null";
                            }
                            else VendorProductSKU = workSheet.Cells[row, 4].Value.ToString();
                            if (workSheet.Cells[row, 5].Value == null)
                            {
                                VendorCategoryId = 0;
                                error += " Category is Null";
                            }
                            else VendorCategoryId = (int)(workSheet.Cells[row, 5].Value);
                            dt1.Rows.Add(Id, ProductName, VendorProductId, VendorProductSKU, VendorCategoryId, error);



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
        public virtual FileResult Download()
        {
            byte[] fileBytes;
            string fileName;


            fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/data1.xlsx"));
            fileName = "ErrorReport.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);



        }

       
        public ActionResult Index(int? page, string search)
        {

            return View();
        }
        [HttpPost]
        public ActionResult ReadFiles()
        {

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

        public virtual FileResult buttonexport()
        {
            string strDelimiter = ",";
            string search = null;
            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            List<WebApi.ServiceReference1.Product> products = client.SelectProduct(search).ToList();
            StringBuilder sb = new StringBuilder();
            sb.Append("Id" + strDelimiter);
            sb.Append("ProductName" + strDelimiter);
            sb.Append("VendorProductId" + strDelimiter);
            sb.Append("VendorProductSKU" + strDelimiter);
            sb.Append("VendorCategoryId" + strDelimiter);

            sb.Append("\r\n");

            foreach (WebApi.ServiceReference1.Product product in products)
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
            byte[] fileBytes;
            string fileName;


            fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/ProductData.csv"));
            fileName = "ProductDetails.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

    }
}
