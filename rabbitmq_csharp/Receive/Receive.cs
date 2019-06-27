using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            //和发送端一样，仍然需要链接服务器
            var factory=new ConnectionFactory{ HostName="localhost"};
            using(var connection=factory.CreateConnection())
            using( var channel=connection.CreateModel())
            {
                channel.QueueDeclare(queue:"hello",
                durable:false,
                exclusive:false,
                autoDelete:false,
                arguments:null);

                var consumer=new EventingBasicConsumer(channel);
                consumer.Received+=(ModuleHandle,ea)=>
                {
                    var body=ea.Body;
                    var message=Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue:"hello", autoAck:true, consumer:consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

            //Console.WriteLine("Hello World!");
        }
    }
}
