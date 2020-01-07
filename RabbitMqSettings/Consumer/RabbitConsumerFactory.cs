using RabbitMqSettings.Setting;

namespace RabbitMqSettings.Consumer
{
    public class RabbitConsumerFactory
    {
        public static RabbitConsumer CreateRabbitConsumer(RabbitSettings configuration)
        {
            RabbitTemplate rabbitTemplate = new RabbitTemplate(configuration);
            rabbitTemplate.Initialize();

            return new RabbitConsumer(rabbitTemplate);
        }

        public static RabbitConsumer CreateRabbitConsumer()
        {
            RabbitSettings configuration = new DefaultRabbitSettings();
            return CreateRabbitConsumer(configuration);
        }
    }
}