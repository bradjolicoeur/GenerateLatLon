using GenerateLatLon.Interfaces;
using GenerateLatLon.Models;
using GenerateLatLon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLatLon
{
    public class PositionGenerationService
    {
        private readonly IEventGenerator _eventGenerator;
        private readonly ICalculateSpeedAndDistance _speedAndDistance;
        private readonly Random rnd = new Random();

        public PositionGenerationService(IEventGenerator eventGenerator, ICalculateSpeedAndDistance speedAndDistance)
        {
            _eventGenerator = eventGenerator;
            _speedAndDistance = speedAndDistance;
        }

        public IEnumerable<IPosition> Generate()
        {
            var sPos = new Coordinates(39.9340, -74.8910);
            var anchorPosition = new Coordinates(39.9340, -74.8910);
            int anchorDistance = 1000;

            DateTime t = DateTime.UtcNow;

            var results = new List<IPosition>();

            for (int i = 0; i < 1000; i++)
            {
                var position = NewPosition(sPos, anchorPosition, anchorDistance);
                sPos.Latitude = position.Latitude;
                sPos.Longitude = position.Longitude;
                position.UtcPositionTime = t.AddMinutes(rnd.Next(1,2));
                results.Add(position);
                ProcessPosition(results, position);
                t = results.Max(m => m.UtcPositionTime);
            }

            return results;
        }

        private Position NewPosition(Coordinates start, Coordinates anchor, int anchorDistanceKM)
        {
            int maxRand = (anchorDistanceKM < 1500) ?  anchorDistanceKM : 1500;
            Position position = null;

            while (true)
            {
                double dn = rnd.Next(maxRand);
                double de = rnd.Next(maxRand);
                position = CalculatePositionHelper.Calculate(start.Latitude, start.Longitude, dn, de);

                double distance = position.DistanceTo(anchor);
                if ( distance <= anchorDistanceKM)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("x distance:" + distance.ToString());
                }
            }
  
            return position;
        }

        private void ProcessPosition(List<IPosition> list, Position position)
        {
            _speedAndDistance.Calulate(position, list.OrderByDescending(o => o.UtcPositionTime).Skip(1).Take(1).FirstOrDefault());

            var events = _eventGenerator.Generate(position);

            if (events != null && events.Count() > 0)
            {
                list.AddRange(events);
            }

        }
    }
}
