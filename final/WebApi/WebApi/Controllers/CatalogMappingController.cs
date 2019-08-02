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
    public class CatalogMappingController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
         [Route("localhost:65126/api/catalogmapping")]
        public HttpResponseMessage Get(String search)
        {
              try
            {
                logger.Info("Catalog Mapping Wpi Get");
            // string search = null;
            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            List<WebApi.ServiceReference1.CatalogMapping> products = client.SelectCatalogMapping(search).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, products);
            }
              catch (Exception e)
              {
                  logger.Error(e, "Error in Catalog Mapping Index");
                  return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "");
              }
        }
         [Route("localhost:65126/api/catalogmapping/InserCatalogMapping")]
         public void InsertCatalogMapping(WebApi.Models.CatalogMapping p)
        {
              try { 
                logger.Info("CatalogMapping Wpi InsertCatalogMapping");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WebApi.Models.CatalogMapping, WebApi.ServiceReference1.CatalogMapping>();
            });

            IMapper mapper = config.CreateMapper();
            WebApi.ServiceReference1.CatalogMapping c = mapper.Map<WebApi.ServiceReference1.CatalogMapping>(p);
            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client();
            if (Object.ReferenceEquals(client, null)) { }
            else
            { client.InsertCatalogMapping(c); }
              }
              catch (Exception e)
              {
                  logger.Error(e, "Error in CatalogMapping Insert Catlog Mapping");

              }
        }
    }
}
