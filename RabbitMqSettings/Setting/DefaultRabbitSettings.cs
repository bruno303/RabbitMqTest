namespace RabbitMqSettings.Setting
{
    public class DefaultRabbitSettings : RabbitSettings
    {
        public DefaultRabbitSettings()
        {
            this.Server = "localhost";
            this.QueueName = "hello";
            this.Prefetch = 1;
            this.ExchangeName = "ex.hello";
            this.ConsumersQuantity = 1;
            this.Port = 6572;
        }
    }
}