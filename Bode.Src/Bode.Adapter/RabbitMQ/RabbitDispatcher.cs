using System.IO;
using RabbitMQ.Client;
using System.Configuration;
using System.Xml.Serialization;
using System;

namespace Bode.Adapter.RabbitMQ
{
    public static class RabbitDispatcher
    {
        private static readonly ConnectionFactory factory;
        static RabbitDispatcher() 
        {
            factory = new ConnectionFactory();
            factory.UserName = ConfigurationManager.AppSettings["RabbitUserName"];
            factory.Password = ConfigurationManager.AppSettings["RabbitUserPsw"];
            factory.HostName = ConfigurationManager.AppSettings["RabbitMQAddress"];
            factory.VirtualHost = ConfigurationManager.AppSettings["RabbitVirtualHost"];
            factory.Port = 5672;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="queueKey">队列的RoutingKey</param>
        /// <param name="data">数据实体</param>
        public static void Dispatch<T>(string queueKey, T data)
        {
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queueKey, true, false, false, null);

                IBasicProperties properties = channel.CreateBasicProperties();

                properties.DeliveryMode = 2;//消息持久化设置，默认为不持久化

                XmlSerializer xs = new XmlSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                xs.Serialize(ms, data);
                byte[] bytes = ms.ToArray();

                channel.BasicPublish("", queueKey, properties, bytes);
            }
        }
    }
}
