using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqSettings.Consumer;
using RabbitMqSettings.Sender;
using RabbitMqSettings.Setting;
using Service;
using Utils;

namespace Configuration
{
    public static class ProjectConfiguration
    {
        public static void ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<AppSettings>(provider =>
            {
                return GetConfigs().GetAwaiter().GetResult();
            });

            serviceCollection.AddSingleton<IMapper>(provider =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<InternalRabbitSettings, RabbitSettings>();
                });
                return config.CreateMapper();
            });

            serviceCollection.AddSingleton<RabbitSettings>(provider =>
            {
                AppSettings appSettings = provider.GetService<AppSettings>();
                IMapper mapper = provider.GetService<IMapper>();
                return mapper.Map<RabbitSettings>(appSettings.Rabbit);
            });

            serviceCollection.AddSingleton<RabbitConsumer>(provider =>
            {
                RabbitSettings rabbitSettings = provider.GetService<RabbitSettings>();
                RabbitConsumer consumer = RabbitConsumerFactory.CreateRabbitConsumer(rabbitSettings);

                return consumer;
            });

            serviceCollection.AddSingleton<RabbitSender>(provider =>
            {
                return RabbitSenderFactory.CreateRabbitSender();
            });

            serviceCollection.AddSingleton<FibonacciService>();
        }

        private static async Task<AppSettings> GetConfigs()
        {
            Console.WriteLine("# Loading settings");
            string json = await FileUtil.ReadFileAsync(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
            Console.WriteLine("# Settings loaded");

            return await JsonUtil.FromJson<AppSettings>(json);
        }
    }
}
