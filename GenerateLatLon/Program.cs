using System;
using GenerateLatLon.Interfaces;
using Newtonsoft.Json;

namespace GenerateLatLon
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new PositionGenerationService(new EventGenerator());

            foreach(var result in service.Generate())
            {
                Console.WriteLine(
                      "lat:" + Math.Round(result.latitude, 6).ToString() 
                    + " lon:" + Math.Round(result.longitude, 6).ToString() 
                    + " time:" + result.UtcPositionTime.ToString() 
                    + " type:" + ((result is IBehaviorEvent)? ((IBehaviorEvent)result).Label : "Position"));
            }
            
            Console.ReadLine();
        }


        
    }
}
