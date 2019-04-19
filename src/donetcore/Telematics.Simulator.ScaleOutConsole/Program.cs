using System;

namespace Telematics.Simulator.ScaleOutConsole
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using NServiceBus.Logging;
    using System;
    using System.Threading.Tasks;
    using Telematics.Simulator.Contracts.Commands;
    using Telematics.Simulator.Core.Models;
    using Telematics.Simulator.EndpointConf;

    class Program
    {
        private static ILog log;

        public static IConfigurationRoot configuration;

        private static IEndpointInstance EndpointInstance { get; set; }

        public const string EndpointName = "ScaleOutEndpoint";

        static async Task Main()
        {
            // Create service collection
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            //Set console title
            Console.Title = EndpointName;

            //Configure logging
            LogManager.Use<DefaultFactory>()
                .Level(NServiceBus.Logging.LogLevel.Info);
            log = LogManager.GetLogger<Program>();

            //Configure NSB Endpoint
            var endpointConfiguration = EndpointConfigurations.ConfigureNSB(serviceCollection, EndpointName);

            //Start NSB Endpoint
            EndpointInstance = await Endpoint.Start(endpointConfiguration);

            //Support Graceful Shut Down of NSB Endpoint in PCF
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            log.Info("ENDPOINT READY");

            for (int i=0; i <= 0; i++)
            {
                log.Info("Starting " + i.ToString());

                await EndpointInstance.Send<InitializeVehicle>(m =>
                {
                    m.StartingPosition = new Coordinates(39.9340, -74.8910);
                    m.Anchor = new Coordinates(39.9340, -74.8910);
                    m.AnchorDistanceKM = 1000;
                    m.AnchorStates = new string[] { "New Jersey", "Pennsylvania", "New York", "Maryland", "Delaware" };
                    m.StartTime = DateTime.UtcNow.AddDays(-1);
                    m.Vehicle = new Vehicle(Guid.NewGuid().ToString());
                });

                System.Threading.Thread.Sleep(5000);
            }

            log.Info("Started fleet");

            Console.Read();

        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (EndpointInstance != null)
            { EndpointInstance.Stop().ConfigureAwait(false); }

            log.Info("Exiting!");
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();

            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
 
        }

    }
}
