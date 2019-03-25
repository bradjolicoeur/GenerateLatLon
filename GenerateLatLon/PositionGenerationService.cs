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

        private int AnchorDistanceKM { get; set; }
        private ICoordinates Anchor { get; set; }
        private IEnumerable<string> AnchorStates { get; set; }

        public PositionGenerationService(IEventGenerator eventGenerator, ICalculateSpeedAndDistance speedAndDistance)
        {
            _eventGenerator = eventGenerator;
            _speedAndDistance = speedAndDistance;
        }

        public IEnumerable<IPosition> Generate(ICoordinates startingPosition, ICoordinates anchor, DateTime startTime, 
            int positions = 1000, int anchorDistanceKM = 1000, string[] anchorStates = null)
        {

            ICoordinates sPos = startingPosition;
            Anchor = anchor;
            AnchorDistanceKM = anchorDistanceKM;
            AnchorStates = anchorStates;

            DateTime t = startTime;

            var results = new List<IPosition>();

            for (int i = 0; i < positions; i++)
            {
                var position = NewPosition(sPos);
                sPos.Latitude = position.Latitude;
                sPos.Longitude = position.Longitude;
                position.UtcPositionTime = t.AddMinutes(rnd.Next(1,2));
                results.Add(position);
                ProcessPosition(results, position);
                t = results.Max(m => m.UtcPositionTime);
            }

            return results;
        }

        private Position NewPosition(ICoordinates start)
        {
            //max rand is % of anchor distance
            int maxRand = Convert.ToInt32(.75 *((AnchorDistanceKM < 1500) ?  AnchorDistanceKM : 1500));
            Position position = null;
            bool boundryViolation = false;
            double dn = 0;
            double de = 0;

            while (true)
            {
                

                //turn the ship 
                if(boundryViolation)
                {
                    dn = dn * -1;
                    boundryViolation = false;
                }else
                {
                    dn = rnd.Next(Convert.ToInt32(maxRand * .5), 1500);// rnd.Next(maxRand);
                    de = rnd.Next(Convert.ToInt32(maxRand * .05));
                }

                Console.WriteLine(dn.ToString() + ", " + de.ToString());
                position = CalculatePositionHelper.Calculate(start.Latitude, start.Longitude, dn, de);

                if (InBoundaries(position))
                {
                    break;
                }
                else
                {
                    boundryViolation = true;
                    if(!(position.DistanceTo(Anchor) <= AnchorDistanceKM))
                    {
                        Console.WriteLine("x too far from anchor " + position.DistanceTo(Anchor).ToString());
                    }
                    else
                    {
                        Console.WriteLine("x Not in boundary:" + position.Latitude.ToString() + ", " + position.Longitude.ToString());
                    }
                    
                }
            }
  
            return position;
        }

        private bool InBoundaries(Position position)
        {
            return (position.DistanceTo(Anchor) <= AnchorDistanceKM)
                && (position.IsPointInState(AnchorStates)); 
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
