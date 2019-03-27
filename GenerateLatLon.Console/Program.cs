using GenerateLatLon;
using GenerateLatLon.Interfaces;
using GenerateLatLon.Models;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLatLonConsole
{
    class Program
    {
        private static EventHubClient HubClient;

        private static readonly Random rnd = new Random();

        static void Main(string[] args)
        {
            CreateEventHubClient();
            var startTime = DateTime.UtcNow.AddDays(-10);

            var Vehicles = new List<GenerateTripRequest>
            {
                new GenerateTripRequest
                {
                    StartingPosition = new Coordinates(39.9340, -74.8910),
                    Anchor = new Coordinates(39.9340, -74.8910),
                    AnchorDistanceKM = 1000,
                    AnchorStates = new string[] { "New Jersey", "Pennsylvania", "New York", "Maryland", "Delaware" },
                    StartTime = startTime,
                    Vehicle = new Vehicle("12VVIELVICE9IDVW89V"),
                    NumberOfPositions = rnd.Next(500, 1000)
                }
                ,  new GenerateTripRequest
                {
                    StartingPosition = new Coordinates(39.9340, -74.8910),
                    Anchor = new Coordinates(39.9340, -74.8910),
                    AnchorDistanceKM = 500,
                    AnchorStates = new string[] { "New Jersey", "Pennsylvania", "New York", "Maryland", "Delaware" },
                    StartTime = startTime,
                    Vehicle = new Vehicle("12VVIELVICE9IDVW89X"),
                    NumberOfPositions = rnd.Next(500, 1000)
                }
                , new GenerateTripRequest
                {
                    StartingPosition = new Coordinates(38.5632, -76.0788),
                    Anchor = new Coordinates(38.5632, -76.0788),
                    AnchorDistanceKM = 1000,
                    //AnchorStates = new string[] { "Pennsylvania", "Virginia", "Maryland", "Delaware" },
                    StartTime = startTime,
                    Vehicle = new Vehicle("12VVIELVICE9IDVW89R"),
                    NumberOfPositions = rnd.Next(500, 1000)
                }
            };

            Parallel.ForEach(Vehicles, v =>
            {
                IPositionGenerationService service = new PositionGenerationService(new EventGenerator(), new CalculateSpeedAndDistance());
                GenerateTrips(v, service);
            });
           
            Console.ReadLine();
        }

        private static void GenerateTrips(IGenerateTripRequest tripRequest, IPositionGenerationService service)
        {

            for (int i = 0; i < 10; i++)
            {
                var positions = service.Generate(tripRequest);
                var last = positions.LastOrDefault();
                tripRequest.StartTime = last.UtcPositionTime.AddHours(1);
                tripRequest.StartingPosition = last;
                foreach (var result in positions)
                {
                    Console.WriteLine(
                         //  "lat:" + Math.Round(result.Latitude, 6).ToString() 
                         //+ " lon:" + Math.Round(result.Longitude, 6).ToString() 
                         //+ " time:" + result.UtcPositionTime.ToString() 
                         " type:" + ((result is IBehaviorEvent) ? ((IBehaviorEvent)result).Label : "Position")
                        + " vehicle:" + result.VehicleId
                        + " distance km:" + result.DistanceKM.ToString()
                        + " speed kph:" + result.SpeedKM.ToString());

                    SendTelemetryEvent(result);
                }
            }
        }

        private static void SendTelemetryEvent(dynamic telemetryReading)
        {
            string serializedString = JsonConvert.SerializeObject(telemetryReading);
            var eventData = new EventData(Encoding.UTF8.GetBytes(serializedString));
            HubClient.SendAsync(eventData);
        }

        private static void CreateEventHubClient()
        {
            var sbNamespace = "Endpoint=sb://drivertelemetry-brad.servicebus.windows.net/;SharedAccessKeyName=generate;SharedAccessKey=AtwWTY9+0Bxzl50CeK3AXCy+DCWKjY79QJfcJHnOw/o=;EntityPath=generatelatlon-v1";
            HubClient = EventHubClient.CreateFromConnectionString(sbNamespace);
        }
    }
}
