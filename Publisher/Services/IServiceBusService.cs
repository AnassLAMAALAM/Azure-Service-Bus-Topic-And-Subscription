namespace Publisher.Services
{
    public interface IServiceBusService
    {
        public Task SendMessageAsync<T>(T ServiceBusMessage, string TopicName);
    }
}
