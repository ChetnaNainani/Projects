using AttributeRouting.Web.Mvc;
using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
namespace WebApi.Controllers
{
    public class CatalogController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [Route("localhost:65126/api/catalog")]
        public HttpResponseMessage Get()
        {
            try { 
                logger.Info("Catalog Wpi Get");
            //string search = null;
            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client("BasicHttpBinding_IService1");
            List<WebApi.ServiceReference1.Catalog> products = client.SelectCatalog().ToList();

            return Request.CreateResponse(HttpStatusCode.OK, products);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error in Catalog Index");
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "");
            }
        }
         [Route("localhost:65126/api/catalog/InsertCatalog")]
        public void InsertCatalog(WebApi.Models.Catalog p)
        {
                 try { 
                logger.Info("Catalog Wpi InsertCatalog");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WebApi.Models.Catalog, WebApi.ServiceReference1.Catalog>();
            });

            IMapper mapper = config.CreateMapper();
         //   var source = new Source();
           // var dest = mapper.Map<Source, Dest>(source);*/
           // Mapper.CreateMap<WebApi.Models.Catalog, WebApi.ServiceReference1.Catalog>();

            WebApi.ServiceReference1.Catalog c = mapper.Map<WebApi.ServiceReference1.Catalog>(p);
        // WebApi.ServiceReference1.Catalog   c = Mapper.Map<WebApi.ServiceReference1.Catalog, Catalog>(p);
            //Catalog c = new Catalog();

            WebApi.ServiceReference1.Service1Client client = new WebApi.ServiceReference1.Service1Client();
            if (Object.ReferenceEquals(client, null)) { }
            else
            { client.InsertCatalog(c); }
                 }
                 catch (Exception e)
                 {
                     logger.Error(e, "Error in Catalog InsertCatalog");
                    
                 }
        }
    }
}
