using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqSettings.Consumer;
using Service;
using Configuration;

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

        private void ConfigureServices()
        {
            Console.WriteLine("# Configuring services");

            _services.ConfigureServices();

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