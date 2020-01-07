using RabbitMqSettings.Setting;

namespace RabbitMqSettings.Sender
{
    public class RabbitSenderFactory
    {
        public static RabbitSender CreateRabbitSender(RabbitSettings configuration)
        {
            RabbitTemplate rabbitTemplate = new RabbitTemplate(configuration);
            rabbitTemplate.Initialize();

            return new RabbitSender(rabbitTemplate);
        }

        public static RabbitSender CreateRabbitSender()
        {
            RabbitSettings configuration = new DefaultRabbitSettings();
            return CreateRabbitSender(configuration);
        }
    }
}