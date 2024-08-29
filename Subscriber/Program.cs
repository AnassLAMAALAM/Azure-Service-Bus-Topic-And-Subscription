using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;

namespace Subscriber
{
    class Program
    {
        static string ConnectionString = "Endpoint=sb://service-bus-anass.servicebus.windows.net/;SharedAccessKeyName=SharedTopicMangementPolicy;SharedAccessKey=ykZzocWumhlZcyDqTgRsIQg7bkxp9n2nY+ASbFkVhmk=";
        static string TopicName = "anass-Topic";
        static string Subscriber = "Anass-Subscription";
        static ISubscriptionClient? SubscriptionClient;

        static async Task Main(string[] args)
        {
            SubscriptionClient = new SubscriptionClient(ConnectionString, TopicName, Subscriber);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = true, // Automatically complete the message processing
                MaxConcurrentCalls = 1 // Process one message at a time
            };

            // Register the message handler
            SubscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            Console.WriteLine("Listening for messages. Press [Enter] to exit.");
            Console.ReadLine(); // Keeps the application running

            // Close the subscription client when the application is exiting
            await SubscriptionClient.CloseAsync();
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            try
            {
                var jsonString = Encoding.UTF8.GetString(message.Body);
                var receivedMessage = JsonSerializer.Deserialize<string>(jsonString);
                Console.WriteLine($"Message Received: {receivedMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message processing exception: {ex.Message}");
            }
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine($"Message handler exception: {args.Exception.Message}");
            return Task.CompletedTask;
        }
    }
}
