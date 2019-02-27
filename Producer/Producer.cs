using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    class Producer
    {
        //TODO: Config file voor hardcoded namen
        static void Main(string[] args)
        {

            //Per exchange moet een nieuwe connection aangemaakt worden
            //TODO: methode og apparte klasse voor schrijven (indien mogelijk)
            var factory = new ConnectionFactory();

            //Vervang localhost door het address waar de RabbitMQ server op draait
            factory.HostName = "localhost";

            //Vervang exchange door de vooraf afgesproken naam van eigen exchange(bvb exchange_kassa)
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "exchange_kassa",
                                        type: "topic");

                SendMessage("Hello world", "guest.checkIn", channel);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

            
        }

        //Vervang string door byte[] voor xml en verwijder de conversie
        static void SendMessage(String message, String key, IModel channel)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "exchange_kassa",
                                 routingKey: key,
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
