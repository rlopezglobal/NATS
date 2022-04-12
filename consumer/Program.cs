using System;
using System.Text;
using System.Threading.Tasks;
using NATS.Client;

namespace consumer
{
    class Program
    {
        private static bool _exit = false;
        private static IConnection _connection;

        static void Main(string[] args)
        {
            using (_connection = ConnectToNats())
            {
                SubscribePubSub();
                SubscribeQueueGroups();
                SubscribeClear();

                Console.Clear();
                Console.WriteLine($"Connected to {_connection.ConnectedUrl}.");
                Console.WriteLine("Consumers started");
                Console.ReadKey(true);
                _exit = true;

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

        private static void SubscribePubSub()
        {
            Task.Run(() =>
            {
                ISyncSubscription sub = _connection.SubscribeSync("nats.pubsubTest");
                while (!_exit)
                {
                    var message = sub.NextMessage();
                    if (message != null)
                    {
                        string data = Encoding.UTF8.GetString(message.Data);
                        LogMessage(data);
                    }
                }
            });
        }

     
        
        private static void SubscribeQueueGroups()
        {
            EventHandler<MsgHandlerEventArgs> handler = (sender, args) =>
            {
                string data = Encoding.UTF8.GetString(args.Message.Data);
                LogMessage(data);
            };

            IAsyncSubscription s = _connection.SubscribeAsync(
                "nats.demo.queuegroupTest", "load-balancing-queue", handler);
        }

       
        private static void LogMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fffffff")} - {message}");
        }

        private static void SubscribeClear()
        {
            EventHandler<MsgHandlerEventArgs> handler = (sender, args) =>
            {
                Console.Clear();
            };

            IAsyncSubscription s = _connection.SubscribeAsync(
                "nats.demo.clear", handler);
        }
    }
}
