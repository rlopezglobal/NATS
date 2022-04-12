using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NATS.Client;

namespace producer
{
    class Program
    {
        private static int _messageCount = 10;
        private static int _sendIntervalMs = 50;
        private const string ALLOWED_OPTIONS = "12qQ";

        private static IConnection _connection;

        static void Main()
        {
            bool exit = false;

            using (_connection = ConnectToNats())
            {
                while (!exit)
                {
                    Console.Clear();

                    Console.WriteLine("NATS demo producer");
                    Console.WriteLine("==================");
                    Console.WriteLine("Select mode:");
                    Console.WriteLine("1) Pub / Sub");
                    Console.WriteLine("2) Load-balancing (queue groups)");
                    Console.WriteLine("q) Quit");

                    // get input
                    ConsoleKeyInfo input;
                    do
                    {
                        input = Console.ReadKey(true);
                    } while (!ALLOWED_OPTIONS.Contains(input.KeyChar));

                    switch (input.KeyChar)
                    {
                        case '1':
                            PubSub();
                            break;
                        case '2':
                            QueueGroups();
                            break;
                        case 'q':
                        case 'Q':
                            exit = true;
                            continue;
                    }

                    Console.WriteLine();
                    Console.WriteLine("Done. Press any key to continue...");
                    Console.ReadKey(true);
                    Clear();
                }

                _connection.Drain(5000);
            }
        }

        private static IConnection ConnectToNats()
        {
            ConnectionFactory factory = new ConnectionFactory();

            var options = ConnectionFactory.GetDefaultOptions();
            options.Url = "nats://localhost:4222";
            
            return factory.CreateConnection(options);
        }

        private static void PubSub()
        {
            Console.Clear();
            Console.WriteLine("Pub/Sub demo");
            Console.WriteLine("============");

            for (int i = 1; i <= _messageCount; i++)
            {
                string message = $"Message {i}";

                Console.WriteLine($"Sending: {message}");

                byte[] data = Encoding.UTF8.GetBytes(message);

                _connection.Publish("nats.pubsubTest", data);

                Thread.Sleep(_sendIntervalMs);
            }
        }

        private static void QueueGroups()
        {
            Console.Clear();
            Console.WriteLine("Load-balancing demo");
            Console.WriteLine("===================");

            for (int i = 1; i <= _messageCount; i++)
            {
                string message = $"Message {i}";

                Console.WriteLine($"Sending: {message}");

                byte[] data = Encoding.UTF8.GetBytes(message);

                _connection.Publish("nats.demo.queuegroupTest", data);

                Thread.Sleep(_sendIntervalMs);
            }
        }

  
        private static void Clear()
        {
            Console.Clear();
            _connection.Publish("nats.demo.clear", null);
        }
    }
}
