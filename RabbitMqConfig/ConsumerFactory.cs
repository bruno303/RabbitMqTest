using System;
using System.IO;
using System.Threading.Tasks;
using Utils;

namespace RabbitMqConfig
{
    public class ConsumerFactory
    {
        public static Consumer CreateConsumer(RabbitMqConfiguration configuration)
        {
            var consumer = new Consumer();
            consumer.Initialize(configuration.Server, configuration.QueueName, configuration.ConsumersQuantity);
            return consumer;
        }

        public static Consumer CreateConsumer()
        {
            RabbitMqConfiguration configuration = GetDefaultConfiguration().GetAwaiter().GetResult();
            var consumer = new Consumer();
            consumer.Initialize(configuration.Server, configuration.QueueName, configuration.ConsumersQuantity);
            return consumer;
        }

        private static async Task<RabbitMqConfiguration> GetDefaultConfiguration()
        {
            string json = await FileUtil.ReadFileAsync(Path.Combine(AppContext.BaseDirectory, "appsettings.default.json"));
            return await JsonUtil.FromJson<RabbitMqConfiguration>(json);
        }
    }
}