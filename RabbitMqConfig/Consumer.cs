using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Linq;

namespace RabbitMqConfig
{
    public class Consumer
    {
        private List<IModel> _channels;
        private IConnection _connection;
        private string _queue;
        public void Initialize(string server, string queueName, int quantity)
        {
            _queue = queueName;
            _connection = new ConnectionFactory().CreateConnection(new List<string>() { server });
            _channels = new List<IModel>();

            for (int i = 0; i < quantity; i++)
            {
                _channels.Add(_connection.CreateModel());
            }

            ConfigureChannels();
        }

        private void ConfigureChannels()
        {
            _channels.AsParallel().ForAll(ch => 
            {
                ch.BasicQos(0, 1, false);
                ch.QueueDeclare(_queue, false, false, true);
            });
        }

        public void SetConsumerEvent(Action<string> method)
        {
            if (_channels == null)
            {
                throw new System.InvalidOperationException("Channel has not been initialized");
            }

            _channels.AsParallel().ForAll(ch =>
            {
                var consumer = new EventingBasicConsumer(ch);
                consumer.Received += (sender, ea) =>
                {
                    string msg = Encoding.UTF8.GetString(ea.Body);
                    method(msg);
                };

                ch.BasicConsume(_queue, true, consumer);
            });
        }
    }
}