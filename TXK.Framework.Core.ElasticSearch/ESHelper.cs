using Elasticsearch.Net;
using Nest;
using System;
using System.Threading;

namespace TXK.Framework.Core.ElasticSearch
{
    public class ESHelper
    {
        private static object locker = new object();
        private static ElasticClient elasticClient;
        private static ElasticLowLevelClient elasticLowLevelClient;

        /// <summary>
        /// 获取索引实例
        /// </summary>
        /// <param name="indexName">索引名称</param>
        /// <returns></returns>
        public static ElasticClient GetClient()
        {
            var uris = new[]{
                       new Uri("http://localhost:9200")
                };
            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool);
            if (elasticClient==null)
            {
                lock (locker)
                {
                    if (elasticClient==null)
                    {
                        elasticClient=new ElasticClient(settings);
                    }
                }
               
            }
            return elasticClient;
        }

        public static ElasticLowLevelClient GetElasticLowLevelClient()
        {
            var uris = new[]{
                       new Uri("http://localhost:9200")
                };
            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool);
            if (elasticLowLevelClient == null)
            {
                lock (locker)
                {
                    if (elasticLowLevelClient == null)
                    {
                        elasticLowLevelClient = new ElasticLowLevelClient(settings);
                    }
                }

            }
            return elasticLowLevelClient;
        }

        public static bool CreateIndex(string indexName, int numberOfReplicas, int numberOfShards)
        {
            if (string.IsNullOrEmpty(indexName) || numberOfReplicas.Equals(0) || numberOfShards.Equals(0))
            {
                throw new ArgumentNullException("参数不能为空");
            }
            try
            {
                var client = GetClient();
                IIndexState indexState = new IndexState()
                {
                    Settings = new IndexSettings()
                    {
                        NumberOfReplicas = numberOfReplicas,//副本数
                        NumberOfShards = numberOfShards//分片数
                    }
                };
                //创建索引 先不maping 
                return client.CreateIndex(indexName, p => p.InitializeUsing(indexState)).IsValid;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static object GetAllType<T>(string indexName) where T : class
        {
            try
            {
                var client = GetClient();
               return client.SearchAsync<T>(s => s
                                   .AllIndices().
                                   AllTypes());
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static bool DeleteIndex(string indexName)
        {
            try
            {
                var client = GetClient();
               return client.DeleteIndex(indexName).IsValid;
            }
            catch (Exception ex)
            {

               // throw;
                return false;
            }
        }

        public static object GetAllIndex()
        {
            try
            {
                var client = GetClient();
                return client.CatIndices();
            }
            catch (Exception)
            {

                throw;
            }
        }

      
    }
}
