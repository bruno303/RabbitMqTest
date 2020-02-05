using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqSettings.Consumer
{
    public class RabbitConsumer
    {
        private RabbitTemplate rabbitTemplate;

        public RabbitConsumer(RabbitTemplate rabbitTemplate)
        {
            this.rabbitTemplate = rabbitTemplate;
        }

        public void SetConsumerEvent(Action<string> method)
        {
            if (rabbitTemplate.Channels == null)
            {
                throw new InvalidOperationException("Channel has not been initialized");
            }

            rabbitTemplate.Channels.AsParallel().ForAll(ch =>
            {
                var consumer = new EventingBasicConsumer(ch);
                consumer.Received += (sender, ea) =>
                {
                    string msg = Encoding.UTF8.GetString(ea.Body);
                    method(msg);
                    ch.BasicAck(ea.DeliveryTag, false);
                };

                ch.BasicConsume(rabbitTemplate.Queue, false, consumer);
            });
        }
    }
}