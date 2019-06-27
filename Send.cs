using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            // 首先要建立一个到rabbitMQ服务器的连接，参数为localhost表示使用本地机器代理，如果要
            //使用其它服务器，则将localhost改为其IP地址即可，这里实际上还有其他参数，只是使用了默认值
            //定义链接工厂
            //API参考地址：https://www.rabbitmq.com/dotnet-api-guide.html#connecting
            var factory=new ConnectionFactory() {HostName="localhost"};
            //通过工厂打开一个链接
            using(var connection=factory.CreateConnection())  //使用using定义范围，在该范围结束时回收资源
            //创建一个信道，大部分API的工作都在此信道上完成
            //为了发送消息，我们需要一个发送消息的频道，用自带的API接口开辟一个通道，这个通道用于首发消息
            using(var channel=connection.CreateModel())
            {
                //声明一个叫hello的队列
                channel.QueueDeclare(queue:"hello",
                durable:false,
                exclusive:false,
                autoDelete:false,
                arguments:null);
                string message="Qin yuan long!";
                var body=Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange:"",
                routingKey:"hello",
                basicProperties:null,
                body:body);
                Console.WriteLine($"[x] Sent {message}");

            }

            
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
