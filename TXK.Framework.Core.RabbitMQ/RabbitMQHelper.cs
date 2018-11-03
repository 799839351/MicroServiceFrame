using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace TXK.Framework.Core.RabbitMQ
{
    public class RabbitMQHelper
    {/// <summary>
     /// RabbitMQHelper
     /// </summary>

        private static ConnectionFactory _connectionFactory = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static RabbitMQHelper()
        {
            _connectionFactory = new ConnectionFactory();
            _connectionFactory.HostName = "localhost";
            _connectionFactory.UserName = "guest";
            _connectionFactory.Password = "guest";
            _connectionFactory.AutomaticRecoveryEnabled = true;
        }

        #region 单消息入队
        /// <summary>
        /// 单消息入队
        /// </summary>
        /// <param name="exchangeName">交换器名称</param>
        /// <param name="exchangeType">交换器类型</param>
        /// <param name="routingKey">路由关键字</param>
        /// <param name="message">消息实例</param>
        public static void Enqueue<TItem>(string exchangeName, string exchangeType, string routingKey, string queueName, TItem message)
        {
            try
            {
                if (message != null)
                {
                    using (IConnection connection = _connectionFactory.CreateConnection())
                    {
                        using (IModel channel = connection.CreateModel())
                        {

                            channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                            channel.QueueDeclare(queueName, true, false, false, null);
                            channel.QueueBind(queueName, exchangeName, routingKey);

                            string messageString = JsonConvert.SerializeObject(message);
                            byte[] body = Encoding.UTF8.GetBytes(messageString);
                            var properties = channel.CreateBasicProperties();
                            properties.Persistent = true; //使消息持久化
                            channel.BasicPublish(exchangeName, routingKey, properties, body);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 消息批量入队
        /// <summary>
        /// 消息批量入队
        /// </summary>
        /// <param name="exchangeName">交换器名称</param>
        /// <param name="exchangeType">交换器类型</param>
        /// <param name="routingKey">路由关键字</param>
        /// <param name="list">消息集合</param>
        public static void Enqueue<TItem>(string exchangeName, string exchangeType, string routingKey, string queueName, List<TItem> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    using (IConnection connection = _connectionFactory.CreateConnection())
                    {
                        using (IModel channel = connection.CreateModel())
                        {
                            foreach (TItem item in list)
                            {
                                if (item != null)
                                {
                                    channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                                    channel.QueueDeclare(queueName, true, false, false, null);
                                    channel.QueueBind(queueName, exchangeName, routingKey);
                                    string messageString = JsonConvert.SerializeObject(item);
                                    byte[] body = Encoding.UTF8.GetBytes(messageString);
                                    var properties = channel.CreateBasicProperties();//使消息持久化
                                    properties.Persistent = true;
                                    channel.BasicPublish(exchangeName, routingKey, properties, body);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion


        #region 消费消息队列
        /// <summary>
        /// 消费消息队列
        /// </summary>
        /// <typeparam name="TItem">消息对象</typeparam>
        /// <param name="exchangeName">交换器名称</param>
        /// <param name="exchangeType">交换器类型</param>
        /// <param name="routingKey">路由关键字</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="func">消费消息的具体操作</param>
        /// <param name="tryTimes">消费失败后，继续尝试消费的次数</param>
        public static void Consume<TItem>(string exchangeName, string exchangeType, string routingKey, string queueName, Func<TItem, bool> func)
        {
            try
            {
               
                using (IConnection connection = _connectionFactory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                        channel.QueueDeclare(queueName, true, false, false, null);
                        channel.QueueBind(queueName, exchangeName, routingKey);
                        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (sender, eventArgs) =>
                         {
                             byte[] body = eventArgs.Body;
                             if (body != null && body.Length > 0)
                             {
                                 string message = Encoding.UTF8.GetString(body);
                                 if (!string.IsNullOrWhiteSpace(message))
                                 {
                                     TItem queueMessage = JsonConvert.DeserializeObject<TItem>(message);
                                     if (queueMessage != null)
                                     {
                                         func(queueMessage);
                                         channel.BasicAck(eventArgs.DeliveryTag, false);
                                         
                                     }
                                 }
                             }
                         };
                        channel.BasicConsume(queueName, false, consumer);
                        Console.ReadLine();//这句话要是少了要了你的亲命
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            #endregion
        }
    }

}
