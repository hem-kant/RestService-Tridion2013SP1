using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DD4T.ContentModel;

namespace DD4T_RestService_2013SP1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract(Name = "GetComponentsBySchemaId")]
        [WebGet(UriTemplate = "/GetComponentsBySchemaId/{pubId}/{SchemaId}")]         
        string[] GetComponentsBySchemaId(string pubId,string SchemaId);
        
        [OperationContract(Name = "GetPageByUrl")]
        [WebGet(UriTemplate = "/GetPageByUrl/{pubId}/{WebMessageFormat}?url={url}")]
        string GetPageByUrl(string pubId,string WebMessageFormat, string url);

        [OperationContract(Name = "GetPageByID")]
        [WebGet(UriTemplate = "/GetPageByID/{pubId}/{TcmID}/{WebMessageFormat}")]
        string  GetPageByID(string pubId, string TcmID, string WebMessageFormat);

        [OperationContract(Name = "GetComponentPresentationByTCMId")]
        [WebGet(UriTemplate = "/GetComponentPresentationByTCMId/{pubId}/{compId}/{WebMessageFormat}")]
        string GetComponentPresentationByTCMId(string pubId, string compId, string WebMessageFormat);

        [OperationContract(Name = "GetmultipleComponentPresentationByTCMId")]
        [WebGet(UriTemplate = "/GetmultipleComponentPresentationByTCMId/{pubId}/{compId}/{WebMessageFormat}")]
        string GetmultipleComponentPresentationByTCMId(string pubId, string compId, string WebMessageFormat);

        [OperationContract(Name = "GetComponentPresentationByPublishedDate")]
        [WebGet(UriTemplate = "/GetComponentPresentationByPublishedDate/{pubId}/{SchemaId}/{WebMessageFormat}")]
        string GetComponentPresentationByPublishedDate(string pubId, string SchemaId, string WebMessageFormat);

        [OperationContract(Name = "GetComponentPresentationBySchemaID")]
        [WebGet(UriTemplate = "/GetComponentPresentationBySchemaID/{pubId}/{SchemaId}/{WebMessageFormat}")]
        string GetComponentPresentationBySchemaID(string pubId, string SchemaId, string WebMessageFormat);

    }
     
}
