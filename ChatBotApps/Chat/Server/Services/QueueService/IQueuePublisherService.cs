namespace Chat.Server.Services.QueueService
{
    public interface IQueuePublisherService
    {
        void Publish(string message);
    }
}
