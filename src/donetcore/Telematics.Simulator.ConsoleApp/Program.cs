using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telematics.Simulator.Core.Interfaces;
using Telematics.Simulator.Core.Models;
using Telematics.Simulator.Core.Services;

namespace Telematics.Simulator.ConsoleApp
{
    class Program
    {

        public static IConfigurationRoot configuration;

        private static readonly Random rnd = new Random();

        private static ILogger _logger;

        //This controls how many trips each vehicle takes per run
        private const int NumberOfTripsToGenerate = 10;

        static void Main(string[] args)
        {

            // Create service collection
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            _logger = loggerFactory.CreateLogger<Program>();
            _logger.LogInformation("Starting application");

            var startTime = DateTime.UtcNow.AddDays(-10);
            List<GenerateTripRequest> Vehicles = GenerateTripRequests(startTime);

            Parallel.ForEach(Vehicles, v =>
            {
                var service = serviceProvider.GetService<IGenerateTripService>();
                service.PositionGenerated += PositionGeneratedHandler;
                service.TripGenerated += TripGeneratedHandler;
                service.GenerateTrips(v, NumberOfTripsToGenerate);
            });
        }

        private static List<GenerateTripRequest> GenerateTripRequests(DateTime startTime)
        {

            //configure vehicles here
            var Vehicles = new List<GenerateTripRequest>
            {
                new GenerateTripRequest
                {
                    StartingPosition = new Coordinates(39.9340, -74.8910), //starting point for first trip
                    Anchor = new Coordinates(39.9340, -74.8910), //anchor point keeps vehicles within a specific radius
                    AnchorDistanceKM = 1000, //radius of the vehicle territory
                    AnchorStates = new string[] { "New Jersey", "Pennsylvania", "New York", "Maryland", "Delaware" }, //optional; keep the vehicle in a set of states
                    StartTime = startTime, //the start time for the first trip
                    Vehicle = new Vehicle(Guid.NewGuid().ToString()), //any identifier works, but something that looks like vin is realistic
                    NumberOfPositions = rnd.Next(500, 1000) //Number of positions to calculate for each trip
                }
                ,  new GenerateTripRequest
                {
                    StartingPosition = new Coordinates(39.9340, -74.8910),
                    Anchor = new Coordinates(39.9340, -74.8910),
                    AnchorDistanceKM = 500,
                    AnchorStates = new string[] { "New Jersey", "Pennsylvania", "New York", "Maryland", "Delaware" },
                    StartTime = startTime,
                    Vehicle = new Vehicle(Guid.NewGuid().ToString()),
                    NumberOfPositions = rnd.Next(500, 1000)
                }
                , new GenerateTripRequest
                {
                    StartingPosition = new Coordinates(38.5632, -76.0788),
                    Anchor = new Coordinates(38.5632, -76.0788),
                    AnchorDistanceKM = 1000,
                    AnchorStates = new string[] { "Pennsylvania", "Virginia", "Maryland", "Delaware" },
                    StartTime = startTime,
                    Vehicle = new Vehicle(Guid.NewGuid().ToString()),
                    NumberOfPositions = rnd.Next(500, 1000)
                }
            };
            return Vehicles;
        }

        static void PositionGeneratedHandler(object source, PositionEventArgs e)
        {
            _logger.LogDebug("Position for" + e.Position.VehicleId);
        }

        static void TripGeneratedHandler(object source, TripEventArgs e)
        {
            _logger.LogInformation("Trip for" + e.TripRequest.Vehicle.VehicleId);
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
            serviceCollection.AddTransient<ICalculateSpeedAndDistance, CalculateSpeedAndDistance>();
            serviceCollection.AddTransient<IEventGenerator, EventGenerator>();
            serviceCollection.AddTransient<IPositionGenerationService, PositionGenerationService>();
            serviceCollection.AddTransient<IGenerateTripService, GenerateTripService>();
        }
    }
}
