using AttributeRouting.Web.Mvc;
using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class CategoryController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [Route("localhost:65126/api/category")]
        public HttpResponseMessage Get()
        {try { 
                logger.Info("Category Wpi Get");
            //string search = null;
            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            List<WebApi.ServiceReference1.Category> products = client.SelectCategory().ToList();

            return Request.CreateResponse(HttpStatusCode.OK, products);
        }
        catch (Exception e)
        {
            logger.Error(e, "Error in Category Index");
            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "");
        }
        }
        [Route("localhost:65126/api/category/InsertCategory")]
        public HttpResponseMessage InsertCategory(WebApi.ServiceReference1.Category p)
        { try { 
                logger.Info("Category Wpi InsertCategory");
           
            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client();
          
             client.InsertCategory(p); 
            return Request.CreateResponse(HttpStatusCode.OK, ""); 
            
        }
        catch (Exception e)
        {
            logger.Error(e, "Error in Category Insert Category");
            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ""); 
        }


           

        }
    }
}
