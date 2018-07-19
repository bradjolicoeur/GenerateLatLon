using GenerateLatLon.Interfaces;
using GenerateLatLon.Models;
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

        public PositionGenerationService(IEventGenerator eventGenerator, ICalculateSpeedAndDistance speedAndDistance)
        {
            _eventGenerator = eventGenerator;
            _speedAndDistance = speedAndDistance;
        }

        public IEnumerable<IPosition> Generate()
        {
            double lat = 39.9340;
            double lon = -74.8910;
            double dn = 100;
            double de = 100;
            DateTime t = DateTime.UtcNow;
            Random rnd = new Random();

            var results = new List<IPosition>();

            for (int i = 0; i < 1000; i++)
            {
                dn = rnd.Next(100,1500);
                de = rnd.Next(100,1500);
                var position = CalculatePositionHelper.Calculate(lat, lon, dn, de);
                lat = position.Latitude;
                lon = position.Longitude;
                position.UtcPositionTime = t.AddMinutes(rnd.Next(1,2));
                results.Add(position);
                ProcessPosition(results, position);
                t = results.Max(m => m.UtcPositionTime);
            }

            return results;
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
