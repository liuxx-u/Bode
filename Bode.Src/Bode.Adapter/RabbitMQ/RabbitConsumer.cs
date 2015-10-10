using System;
using System.IO;
using RabbitMQ.Client;
using System.Configuration;
using System.Xml.Serialization;
using RabbitMQ.Client.Events;

namespace Bode.Adapter.RabbitMQ
{
    public class RabbitConsumer
    {
        private readonly ConnectionFactory factory;
        public RabbitConsumer()
        {
            factory = new ConnectionFactory();
            factory.UserName = ConfigurationManager.AppSettings["RabbitUserName"];
            factory.Password = ConfigurationManager.AppSettings["RabbitUserPsw"];
            factory.HostName = ConfigurationManager.AppSettings["RabbitMQAddress"];
            factory.VirtualHost = ConfigurationManager.AppSettings["RabbitVirtualHost"];
            factory.Port = 5672;
        }

        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="queueKey">队列的RoutingKey</param>
        /// <param name="handler">处理委托</param>
        public T Consume<T>(string queueKey, Action<T> handler)
        {
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueKey, true, false, false, null);
                    //channel.BasicQos(500, 1, true);
                    QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueKey, true, consumer);

                    while (true)
                    {
                        BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        byte[] bytes = ea.Body;

                        XmlSerializer xs = new XmlSerializer(typeof(T));
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            T data = (T)xs.Deserialize(ms);
                            handler(data);
                        }
                    }
                }
            }
        }
    }
}
