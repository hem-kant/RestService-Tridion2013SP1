﻿using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Providers;
using DD4T.Factories;
using DD4T.Providers.SDLTridion2013;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Tridion.ContentDelivery.DynamicContent;
using Tridion.ContentDelivery.DynamicContent.Query;

namespace DD4T_RestService_2013SP1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : BaseProvider, IService1
    {
        private PageContentAssembler _pageContentAssember = null;
        private PageContentAssembler PageContentAssembler
        {
            get
            {
                if (_pageContentAssember == null)
                    _pageContentAssember = new PageContentAssembler();
                return _pageContentAssember;
            }
        }
         
        XmlDocument doc = new XmlDocument();
        #region Get Components by SchemaID
        /// <summary>
        /// Get Components by SchemaID
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetComponentsBySchemaId/{publid}/{SchemaId}
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetComponentsBySchemaId/2073/3174
        /// </summary>
        /// <param name="pubId"></param>
        /// <param name="SchemaId"></param>
        /// <returns></returns>
        public string[] GetComponentsBySchemaId(string pubId, string SchemaId)
        {
            var PublicationId = pubId;

            string[] resultUris = null;
            using (var pageQuery = new Query())
            {

                using (var isPage = new ItemTypeCriteria(16))
                {
                    using (var pageUrl = new ItemSchemaCriteria(int.Parse(SchemaId)))
                    {
                        using (var allCriteria = CriteriaFactory.And(isPage, pageUrl))
                        {
                            if (int.Parse(PublicationId) != 0)
                            {
                                using (var correctSite = new PublicationCriteria(int.Parse(PublicationId)))
                                {
                                    allCriteria.AddCriteria(correctSite);
                                }
                            }

                            pageQuery.Criteria = allCriteria;
                        }
                    }
                }

                resultUris = pageQuery.ExecuteQuery();


            }
            return resultUris;

        }
        #endregion

        #region Get Page by URL
        /// <summary>
        /// get Page by URL
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetPageByUrl/{publid}/{WebMessageFormat}?url={WebMessageFormat}
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetPageByUrl/2073/xml?url=/Collection/index.aspx
        /// </summary>
        /// <param name="pubId"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string GetPageByUrl(string pubId, string WebMessageFormat, string Url)
        {
            var PublicationId = pubId;
            string retVal = string.Empty;
            string[] resultUris = null;
            using (var pageQuery = new Query())
            {

                using (var isPage = new ItemTypeCriteria(64))
                {
                    using (var pageUrl = new PageURLCriteria(Url))
                    {
                        using (var allCriteria = CriteriaFactory.And(isPage, pageUrl))
                        {
                            if (int.Parse(PublicationId) != 0)
                            {
                                using (var correctSite = new PublicationCriteria(int.Parse(PublicationId)))
                                {
                                    allCriteria.AddCriteria(correctSite);
                                }
                            }

                            pageQuery.Criteria = allCriteria;
                        }
                    }
                }

                resultUris = pageQuery.ExecuteQuery();
                retVal = PageContentAssembler.GetContent(resultUris[0]);
                doc.LoadXml(retVal);
                if (WebMessageFormat.ToLower() == "json")
                {
                    retVal = JsonConvert.SerializeXmlNode(doc);
                }
                else
                {
                    retVal = doc.InnerXml;
                }
            }
            return retVal;

        }
        #endregion

        #region Get Page by TcmId
        /// <summary>
        /// Get Page by TcmId
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetPageByID/{publid}/{TcmID}/{WebMessageFormat}
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetPageByID/2073/11423/xml
        /// </summary>
        /// <param name="pubId"></param>
        /// <param name="TcmID"></param>
        /// <returns></returns>
        public string GetPageByID(string pubId, string TcmID, string WebMessageFormat)
        {

            string retVal = string.Empty;
            if (TcmID != null)
            {

                retVal = PageContentAssembler.GetContent("tcm:" + pubId + "-" + TcmID + "-64");
                doc.LoadXml(retVal);
                if (WebMessageFormat.ToLower() == "json")
                {
                    retVal = JsonConvert.SerializeXmlNode(doc);
                }
                else
                {
                    retVal = doc.InnerXml;
                }


            }
            return retVal;

        }
        #endregion

        #region Get ComponentPresentation By TCMId
        /// <summary>
        /// Get ComponentPresentation By TCMId
        ///  http://localhost:62377/Tridion2013SP1Provider.svc/GetComponentPresentationByTCMId/{publid}/{TcmID}/{WebMessageFormat}
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetComponentPresentationByTCMId/1048/9558/xml
        /// </summary>
        /// <param name="pubId"></param>
        /// <param name="compId"></param>
        /// <param name="WebMessageFormat"></param>
        /// <returns></returns>
        public string GetComponentPresentationByTCMId(string pubId, string compId, string WebMessageFormat)
        {
            var PublicationId = pubId;
            DD4T.ContentModel.IComponent comp = null;
            ComponentFactory factory = new ComponentFactory();
            string item = "tcm:" + pubId + "-" + compId + "-16";
            factory.TryGetComponent(item, out comp);
            string component = JsonConvert.SerializeObject(comp);
            return component;
        }
        #endregion

        #region Get multipleComponentPresentation By TCMId 
        /// <summary>
        /// Get multipleComponentPresentation By TCMId Compid(,)sepreated 9558,9559
        ///  http://localhost:62377/Tridion2013SP1Provider.svc/GetmultipleComponentPresentationByTCMId/{publid}/{TcmID}/{WebMessageFormat}
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetmultipleComponentPresentationByTCMId/1048/9558,9559/xml
        /// </summary>
        /// <param name="pubId"></param>
        /// <param name="compId"></param>
        /// <param name="WebMessageFormat"></param>
        /// <returns></returns>
        public string GetmultipleComponentPresentationByTCMId(string pubId, string compId, string WebMessageFormat)
        {
            var PublicationId = pubId;
            string component = string.Empty;
            List<IComponent> compo = new List<IComponent>();
            DD4T.ContentModel.IComponent comp = null;
            ComponentFactory factory = new ComponentFactory();
            string[] componentIDs = compId.Split(',');
            foreach (var componentID in componentIDs)
            {
                string item = "tcm:" + pubId + "-" + componentID + "-16";
                factory.TryGetComponent(item, out comp);
                compo.Add(comp);
            }
            if (WebMessageFormat.ToLower() == "json")
            {
                component = JsonConvert.SerializeObject(compo);
            }
            else
            {
                component = JsonConvert.SerializeObject(compo);
                XmlDocument doc = new XmlDocument();
                using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(component), XmlDictionaryReaderQuotas.Max))
                {
                    XElement xml = XElement.Load(reader);
                    doc.LoadXml(xml.ToString());
                }
                component = doc.InnerXml.ToString();
            }

            return component;
        }
        #endregion

        #region Get ComponentPresentation By PublishedDate
        /// <summary>
        /// Get ComponentPresentation By PublishedDate
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetComponentPresentationByPublishedDate/{publid}/{SchemaId}/{WebMessageFormat}
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetComponentPresentationByPublishedDate/1048/3174/xml
        /// </summary>
        /// <param name="pubId"></param>
        /// <param name="SchemaId"></param>
        /// <param name="WebMessageFormat"></param>
        /// <returns></returns>
        public string GetComponentPresentationByPublishedDate(string pubId, string SchemaId, string WebMessageFormat)
        {

            string[] resultUris = null;
            string component = string.Empty;
            List<IComponent> compo = new List<IComponent>();
            using (var pageQuery = new Query())
            {

                using (var isPage = new ItemTypeCriteria(16))
                {
                    using (var pageUrl = new ItemSchemaCriteria(int.Parse(SchemaId)))
                    {
                        using (var allCriteria = CriteriaFactory.And(isPage, pageUrl))
                        {
                            if (int.Parse(pubId) != 0)
                            {
                                using (var correctSite = new PublicationCriteria(int.Parse(pubId)))
                                {
                                    allCriteria.AddCriteria(correctSite);
                                }
                            }

                            pageQuery.Criteria = allCriteria;
                            SortParameter sortParameter = new SortParameter(
                            SortParameter.ItemLastPublishedDate,
                            SortParameter.Descending);
                            pageQuery.AddSorting(sortParameter);
                        }
                    }
                }

                resultUris = pageQuery.ExecuteQuery();


            }
            if (resultUris.Length > 0)
            {
                DD4T.ContentModel.IComponent comp = null;
                ComponentFactory factory = new ComponentFactory();
                foreach (var items in resultUris)
                {
                    factory.TryGetComponent(items, out comp);
                    compo.Add(comp);
                }
                if (WebMessageFormat.ToLower() == "json")
                {
                    component = JsonConvert.SerializeObject(compo);
                }
                else
                {
                    component = JsonConvert.SerializeObject(compo);
                    XmlDocument doc = new XmlDocument();
                    using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(component), XmlDictionaryReaderQuotas.Max))
                    {
                        XElement xml = XElement.Load(reader);
                        doc.LoadXml(xml.ToString());
                    }
                    component = doc.InnerXml.ToString();
                }
            }

            return component;
        }
        #endregion

        #region Get ComponentPresentation By SchemaID
        /// <summary>
        /// Get ComponentPresentation By SchemaID
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetComponentPresentationBySchemaID/{publid}/{SchemaId}/{WebMessageFormat}
        /// http://localhost:62377/Tridion2013SP1Provider.svc/GetComponentPresentationBySchemaID/1048/3174/xml
        /// </summary>
        /// <param name="pubId"></param>
        /// <param name="SchemaId"></param>
        /// <param name="WebMessageFormat"></param>
        /// <returns></returns>
        public string GetComponentPresentationBySchemaID(string pubId, string SchemaId, string WebMessageFormat)
        {

            string[] resultUris = null;
            string component = string.Empty;
            List<IComponent> compo = new List<IComponent>();
            using (var pageQuery = new Query())
            {

                using (var isPage = new ItemTypeCriteria(16))
                {
                    using (var pageUrl = new ItemSchemaCriteria(int.Parse(SchemaId)))
                    {
                        using (var allCriteria = CriteriaFactory.And(isPage, pageUrl))
                        {
                            if (int.Parse(pubId) != 0)
                            {
                                using (var correctSite = new PublicationCriteria(int.Parse(pubId)))
                                {
                                    allCriteria.AddCriteria(correctSite);
                                }
                            } 
                        }
                    }
                }

                resultUris = pageQuery.ExecuteQuery();


            }
            if (resultUris.Length > 0)
            {
                DD4T.ContentModel.IComponent comp = null;
                ComponentFactory factory = new ComponentFactory();
                foreach (var items in resultUris)
                {
                    factory.TryGetComponent(items, out comp);
                    compo.Add(comp);
                }
                if (WebMessageFormat.ToLower() == "json")
                {
                    component = JsonConvert.SerializeObject(compo);
                }
                else
                {
                    component = JsonConvert.SerializeObject(compo);
                    XmlDocument doc = new XmlDocument();
                    using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(component), XmlDictionaryReaderQuotas.Max))
                    {
                        XElement xml = XElement.Load(reader);
                        doc.LoadXml(xml.ToString());
                    }
                    component = doc.InnerXml.ToString();
                }
            }

            return component;
        }
        #endregion

    }

}
