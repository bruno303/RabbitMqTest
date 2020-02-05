using System;
using System.Threading;
using Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqSettings.Sender;

namespace Producer
{
    public class Application
    {
        private ServiceProvider _serviceProvider;
        private IServiceCollection _services = new ServiceCollection();

        public Application()
        {
            Console.WriteLine("# Starting application");
            ConfigureServices();
            _serviceProvider = _services.BuildServiceProvider();
            Console.WriteLine("# Service provider ready");
        }

        private void ConfigureServices()
        {
            Console.WriteLine("# Configuring services");

            _services.ConfigureServices();

            Console.WriteLine("# Services configured");
        }

        public void Run()
        {
            Console.WriteLine("# Application initialized");

            var random = new Random();
            RabbitSender rabbitSender = _serviceProvider.GetService<RabbitSender>();

            while (true)
            {
                int num = random.Next(0, 50);
                rabbitSender.SendToQueue(num.ToString());
                Console.WriteLine($"# Value sended {num}");
                
                Thread.Sleep(1000);
            }
        }
    }
}