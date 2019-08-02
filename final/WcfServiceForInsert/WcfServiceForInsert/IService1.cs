using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceForInsert
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        void InsertProduct(Product product);
        [OperationContract]
        List<Product> SelectProduct(string search);
       
        [OperationContract]
        void InsertCatalogMapping(CatalogMapping product);
        [OperationContract]
        List<CatalogMapping> SelectCatalogMapping(string search);

        [OperationContract]
        void InsertCatalog(Catalog product);
        [OperationContract]
        List<Catalog> SelectCatalog();

        [OperationContract]
        void InsertCategory(Category product);
        [OperationContract]
        List<Category> SelectCategory();
        
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    
}
