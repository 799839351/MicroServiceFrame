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
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            for (int i = 0; i < 10; i++)
            {
                RabbitMQHelper.Enqueue("txk", "direct", "woaini","nihao", new User() { Id = 1, Provence = "陕西" });

            }
            RabbitMQHelper.Consume<User>("txk", "direct", "woaini", "nihao", (o) =>
             {
                 string message = JsonConvert.SerializeObject(o);
                 Console.Write(message);
                 return true;
             });


           Console.ReadKey();

        }

     
      }
}
