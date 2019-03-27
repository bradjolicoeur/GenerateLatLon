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

        //This controls how many trips each vehicle takes per run
        private const int NumberOfTripsToGenerate = 10;

        private static void CreateEventHubClient()
        {
            //TODO: add your connection string here
            var sbNamespace = "[Event Hub Connection String Goes Here]";
            HubClient = EventHubClient.CreateFromConnectionString(sbNamespace);
        }

        static void Main(string[] args)
        {
            CreateEventHubClient();
            var startTime = DateTime.UtcNow.AddDays(-10);

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
                    Vehicle = new Vehicle("12VVIELVICE9IDVW89V"), //any identifier works, but something that looks like vin is realistic
                    NumberOfPositions = rnd.Next(500, 1000) //Number of positions to calculate for each trip
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

            for (int i = 0; i < NumberOfTripsToGenerate; i++)
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

       
    }
}
