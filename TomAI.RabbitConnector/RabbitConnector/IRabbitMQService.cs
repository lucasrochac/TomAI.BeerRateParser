namespace TomAI.RabbitConnector.RabbitConnector
{
    public interface IRabbitMQService
    {
        void SendMessage(string message);
    }
}