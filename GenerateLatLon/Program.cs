using System;
using GenerateLatLon.Interfaces;
using Newtonsoft.Json;

namespace GenerateLatLon
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new PositionGenerationService(new EventGenerator(), new CalculateSpeedAndDistance());

            foreach(var result in service.Generate())
            {
                Console.WriteLine(
                    //  "lat:" + Math.Round(result.Latitude, 6).ToString() 
                    //+ " lon:" + Math.Round(result.Longitude, 6).ToString() 
                    //+ " time:" + result.UtcPositionTime.ToString() 
                     " type:" + ((result is IBehaviorEvent)? ((IBehaviorEvent)result).Label : "Position")
                    + " distance km:" + result.DistanceKM.ToString()
                    + " speed kph:" + result.SpeedKM.ToString());
            }
            
            Console.ReadLine();
        }


        
    }
}
