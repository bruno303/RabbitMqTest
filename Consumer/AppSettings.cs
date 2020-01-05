namespace Consumer
{
    public class AppSettings
    {
        public RabbitSettings Rabbit { get; set; }
    }
    public class RabbitSettings
    {
        public string Server { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public int ConsumersQuantity { get; set; }
        public int Prefetch { get; set; }
    }
}