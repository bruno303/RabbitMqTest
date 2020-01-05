namespace RabbitMqConfig
{
    public class RabbitMqConfiguration
    {
        public string Server { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public int ConsumersQuantity { get; set; }
        public int Prefetch { get; set; }
    }
}