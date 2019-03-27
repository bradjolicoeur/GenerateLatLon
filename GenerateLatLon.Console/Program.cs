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

            IPositionGenerationService service = new PositionGenerationService(new EventGenerator(), new CalculateSpeedAndDistance());

            ICoordinates sPos = new Coordinates(39.9340, -74.8910);
            var Anchor = new Coordinates(39.9340, -74.8910);
            var anchorStates = new string[] { "New Jersey", "Pennsylvania", "New York", "Maryland", "Delaware" };
            var startTime = DateTime.UtcNow.AddDays(-10);
            var vehicle = new Vehicle("12VVIELVICE9IDVW89V");
            int tripPositions = rnd.Next(500, 1000);

            for (int i = 0; i < 10; i++)
            {
                var positions = service.Generate(vehicle, sPos, Anchor, startTime, tripPositions, 1000, null);
                var last = positions.LastOrDefault();
                startTime = last.UtcPositionTime.AddHours(1);
                sPos = last;
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
            Console.ReadLine();
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
