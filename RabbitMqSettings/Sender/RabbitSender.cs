using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMqSettings.Abstract;

namespace RabbitMqSettings.Sender
{
    public class RabbitSender : AbstractRabbitService
    {
        private RabbitTemplate rabbitTemplate;

        public RabbitSender(RabbitTemplate rabbitTemplate): base(rabbitTemplate)
        {
            this.rabbitTemplate = rabbitTemplate;
        }

        public void SendToQueue(string msg)
        {
            rabbitTemplate.Channels.First().BasicPublish(string.Empty, rabbitTemplate.Queue, null, Encoding.UTF8.GetBytes(msg));
        }

        public void SendToQueue(string msg, string queue)
        {
            rabbitTemplate.Channels.First().BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(msg));
        }

        public void SendToExchange(string msg, string exchange, string routingKey)
        {
            rabbitTemplate.Channels.First().BasicPublish(exchange, routingKey, false, null, Encoding.UTF8.GetBytes(msg));
        }
    }
}