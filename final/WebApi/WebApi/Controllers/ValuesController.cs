using AttributeRouting;
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
    [RoutePrefix("api/[controller]")]
    public class ValuesController : ApiController
    {
        // GET api/values
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [Route("localhost:65126/api/values")]
        public HttpResponseMessage Get(String search)
        {
            try
            {
                logger.Info("Home Wpi Get");
                // string search = null;
                WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
                List<WebApi.ServiceReference1.Product> products = client.SelectProduct(search).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, products);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Home Index");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "");
            }
        }
        [Route("localhost:65126/api/values/InsertProduct")]
        public void InsertProduct(WebApi.Models.Product p)
        {
            try { 
                logger.Info("Home Wpi InsertProduct");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WebApi.Models.Product, WebApi.ServiceReference1.Product>();
            });

            IMapper mapper = config.CreateMapper();
            WebApi.ServiceReference1.Product c = mapper.Map<WebApi.ServiceReference1.Product>(p);
            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client();
            if (Object.ReferenceEquals(client, null)) { }
            else
            { client.InsertProduct(c); }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Home Insert Product");

            }
           
        }
        
       

    }
}