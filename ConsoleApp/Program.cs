using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using TXK.Framework.Core.ElasticSearch;
using TXK.Framework.Core.Model;
using TXK.Framework.Core.RabbitMQ;

namespace ConsoleApp
{
    class Program
    {
        static  void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            #region 测试


            //for (int i = 0; i < 10; i++)
            //{
            //    RabbitMQHelper.Enqueue("txk", "direct", "woaini", "nihao", new User() { Id = 1, Provence = "陕西" });

            //}
            //RabbitMQHelper.Consume<User>("txk", "direct", "woaini", "nihao", (o) =>
            // {
            //     string message = JsonConvert.SerializeObject(o);
            //     Console.Write(message);
            //     return true;
            // });

            //foreach (var item in ESHelper.GetAllIndex().Records)
            //{
            //    ESHelper.DeleteIndex(item.Index);
            //};

            //var client = ESHelper.GetClient();
            //var t = client.Index(new ESModel { Id = 5, Name = "谭小康", Birthday = DateTime.Now, Age = 168 }, m => m.Index("myindex").Type("esmodel"));
            //var t2 = client.Index(new ESModel { Id = 6, Name = "谭小康", Birthday = DateTime.Now, Age = 186 }, m => m.Index("myindex").Type("esmodel"));

            //var result = client.Search<ESModel>(q => q.Index("myindex").From(0).Size(5).Collapse(r => r.Field(f => f.Age))
            // );
            //var lowclient = ESHelper.GetElasticLowLevelClient();
            //lowclient.Search<StringResponse>("myindex", PostData.String(@""));
            #endregion
            M();
            Console.ReadKey();

        }
        static async void M()
        {
            Test test = new Test();
            await test.M1();
             test.M2();
        }
     
      }
}
