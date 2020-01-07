using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;
using RabbitMqSettings.Setting;

namespace RabbitMqSettings
{
    public class RabbitTemplate
    {
        private IConnection _connection;
        private RabbitSettings _settings;

        public List<IModel> Channels { get; private set; }
        public string Queue { get; private set; }

        public RabbitTemplate(RabbitSettings settings)
        {
            this._settings = settings;
        }

        public RabbitTemplate()
        {
            this._settings = new DefaultRabbitSettings();
        }
        
        public void Initialize()
        {
            Queue = _settings.QueueName;
            _connection = new ConnectionFactory().CreateConnection(new List<string>() { _settings.Server });
            Channels = new List<IModel>();

            for (int i = 0; i < _settings.ConsumersQuantity; i++)
            {
                Channels.Add(_connection.CreateModel());
            }

            ConfigureChannels();
        }

        private void ConfigureChannels()
        {
            Channels.AsParallel().ForAll(ch => 
            {
                ch.BasicQos(0, Convert.ToUInt16(_settings.Prefetch), false);
                ch.QueueDeclare(Queue, false, false, true);
            });
        }
    }
}