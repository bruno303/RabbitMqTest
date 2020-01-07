using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqSettings.Consumer;
using RabbitMqSettings.Setting;
using Service;
using Utils;

namespace Consumer
{
    public class Application
    {
        private IServiceCollection _services = new ServiceCollection();
        private ServiceProvider _serviceProvider;

        public Application()
        {
            Console.WriteLine("# Starting application");
            ConfigureServices();
            _serviceProvider = _services.BuildServiceProvider();
            Console.WriteLine("# Service provider ready");
        }

        private async Task<AppSettings> GetConfigs()
        {
            Console.WriteLine("# Loading settings");
            string json = await FileUtil.ReadFileAsync(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
            Console.WriteLine("# Settings loaded");

            return await JsonUtil.FromJson<AppSettings>(json);
        }

        private void ConfigureServices()
        {
            Console.WriteLine("# Configuring services");

            _services.AddSingleton<AppSettings>(provider =>
            {
                return GetConfigs().GetAwaiter().GetResult();
            });

            _services.AddSingleton<IMapper>(provider =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<InternalRabbitSettings, RabbitSettings>();
                });
                return config.CreateMapper();
            });

            _services.AddSingleton<RabbitSettings>(provider =>
            {
                AppSettings appSettings = provider.GetService<AppSettings>();
                IMapper mapper = provider.GetService<IMapper>();
                return mapper.Map<RabbitSettings>(appSettings.Rabbit);
            });

            _services.AddSingleton<RabbitConsumer>(provider =>
            {
                RabbitSettings rabbitSettings = provider.GetService<RabbitSettings>();
                RabbitConsumer consumer = RabbitConsumerFactory.CreateRabbitConsumer(rabbitSettings);

                return consumer;
            });

            _services.AddSingleton<FibonacciService>();

            Console.WriteLine("# Services configured");
        }

        public void Run()
        {
            RabbitConsumer rabbitConsumer = _serviceProvider.GetService<RabbitConsumer>();
            rabbitConsumer.SetConsumerEvent(EventHandler);

            Console.WriteLine("# Application initialized");
        }

        private void EventHandler(string msg)
        {
            FibonacciService fibonacciService = _serviceProvider.GetService<FibonacciService>();

            Task.Run(() =>
            {
                long num = Convert.ToInt64(msg);

                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"# Fibonacci of {num} = {fibonacciService.CalculateFibonacci(num)}");
            });
        }
    }


}