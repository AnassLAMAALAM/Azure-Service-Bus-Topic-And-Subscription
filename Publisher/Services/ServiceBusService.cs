using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;
namespace Publisher.Services
{
    public class ServiceBusService : IServiceBusService
    {
        public IConfiguration config { get; set; }
        public ITopicClient topicClient { get; set; }

        public ServiceBusService(IConfiguration config, ITopicClient topicClient) 
        {
            this.config = config;
            this.topicClient = topicClient;
        }

        public async Task SendMessageAsync<T>(T ServiceBusMessage, string TopicName)
        {
            try
            {
                var ConnectionString = this.config.GetConnectionString("AzureServiceBus");
                string MessageBody = JsonSerializer.Serialize(ServiceBusMessage);
                var Message = new Message(Encoding.UTF8.GetBytes(MessageBody));
                await this.topicClient.SendAsync(Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await topicClient.CloseAsync();
            }
        }

    }
}
