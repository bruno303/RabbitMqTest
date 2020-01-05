using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqConfig;
using Utils;

namespace Consumer
{
    public class Application
    {
        private IServiceCollection _services = new ServiceCollection();
        private ServiceProvider _serviceProvider;

        public Application()
        {
            ConfigureServices();
            _serviceProvider = _services.BuildServiceProvider();
        }

        private async Task<AppSettings> GetConfigs()
        {
            string json = await FileUtil.ReadFileAsync(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
            return await JsonUtil.FromJson<AppSettings>(json);
        }

        private void ConfigureServices()
        {
            // _services.AddSingleton<AppSettings>(provider =>
            // {
            //     return GetConfigs().GetAwaiter().GetResult();
            // });

            // _services.AddSingleton<IMapper>(provider =>
            // {
            //     var config = new MapperConfiguration(cfg =>
            //     {
            //         cfg.CreateMap<RabbitSettings, RabbitMqConfiguration>();
            //     });
            //     return config.CreateMapper();
            // });

            _services.AddSingleton<RabbitMqConfig.Consumer>(provider =>
            {
                AppSettings appSettings = provider.GetService<AppSettings>();
                IMapper mapper = provider.GetService<IMapper>();
                // RabbitMqConfig.Consumer consumer = ConsumerFactory.CreateConsumer(mapper.Map<RabbitMqConfiguration>(appSettings.Rabbit));
                RabbitMqConfig.Consumer consumer = ConsumerFactory.CreateConsumer();
                consumer.SetConsumerEvent(msg =>
                {
                    Console.WriteLine(msg);
                });

                return consumer;
            });
        }

        public void Run()
        {
            _serviceProvider.GetService<RabbitMqConfig.Consumer>();
            Console.WriteLine("Application initialized...");
        }
    }


}