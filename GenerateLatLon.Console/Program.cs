using GenerateLatLon;
using GenerateLatLon.Interfaces;
using GenerateLatLon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLatLonConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IPositionGenerationService service = new PositionGenerationService(new EventGenerator(), new CalculateSpeedAndDistance());

            ICoordinates sPos = new Coordinates(39.9340, -74.8910);
            var Anchor = new Coordinates(39.9340, -74.8910);
            var anchorStates = new string[] { "New Jersey", "Pennsylvania", "New York", "Maryland", "Delaware" };
            var startTime = DateTime.UtcNow.AddDays(-10);

            for (int i = 0; i < 4; i++)
            {
                var positions = service.Generate(sPos, Anchor, startTime, 500, 1000, null);
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
                        + " distance km:" + result.DistanceKM.ToString()
                        + " speed kph:" + result.SpeedKM.ToString());
                }
            }
            Console.ReadLine();
        }
    }
}
