using Telematics.Simulator.Core.Interfaces;
using Telematics.Simulator.Core.Models;
using Telematics.Simulator.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Core.Services
{
    public class PositionGenerationService : IPositionGenerationService
    {
        private readonly IEventGenerator _eventGenerator;
        private readonly ICalculateSpeedAndDistance _speedAndDistance;
        private readonly Random _rnd;

        private int AnchorDistanceKM { get; set; }
        private ICoordinates Anchor { get; set; }
        private IEnumerable<string> AnchorStates { get; set; }
        private IVehicle Vehicle { get; set; }
        private readonly ILogger _log;

        public PositionGenerationService(IEventGenerator eventGenerator, ICalculateSpeedAndDistance speedAndDistance
                                        , ILoggerFactory loggerFactory, IRandomFactory randomFactory)
        {
            _eventGenerator = eventGenerator;
            _speedAndDistance = speedAndDistance;
            _log = loggerFactory.CreateLogger<GenerateTripService>();
            _rnd = randomFactory.Create();
        }

        public IEnumerable<IPosition> Generate(IGenerateTripRequest tripRequest)
        {
            return Generate(tripRequest.Vehicle, tripRequest.StartingPosition, tripRequest.Anchor, tripRequest.StartTime,
                tripRequest.NumberOfPositions, tripRequest.AnchorDistanceKM, tripRequest.AnchorStates);
        }

        public IEnumerable<IPosition> Generate(IVehicle vehicle, ICoordinates startingPosition, ICoordinates anchor, DateTime startTime, 
            int positions = 1000, int anchorDistanceKM = 1000, string[] anchorStates = null)
        {
            Vehicle = vehicle;
            ICoordinates sPos = startingPosition;
            Anchor = anchor;
            AnchorDistanceKM = anchorDistanceKM;
            AnchorStates = anchorStates;
            double dn = 0;
            double de = 0;

            DateTime t = startTime;

            var results = new List<IPosition>();
            bool holdHeading = false;
            int holdHeadingCount = 0;

            for (int i = 0; i < positions; i++)
            {
                
                var position = NewPosition(sPos, ref holdHeading, dn, de);
                dn = position.dn;
                de = position.de;

                if(holdHeading)
                {
                    if(holdHeadingCount < 15)
                    {
                        holdHeadingCount++;
                    }else
                    {
                        holdHeading = false;
                        holdHeadingCount = 0;
                    }
                }

                position.VehicleId = Vehicle.VehicleId;
                sPos.Latitude = position.Latitude;
                sPos.Longitude = position.Longitude;
                position.UtcPositionTime = t.AddMinutes(_rnd.Next(1,2));
                results.Add(position);
                ProcessPosition(results, position);
                t = results.Max(m => m.UtcPositionTime);
            }

            foreach (var item in results.Where(v => v.VehicleId == null))
            {
                item.VehicleId = Vehicle.VehicleId;
            }

            return results;
        }

        private IPosition NewPosition(ICoordinates start,ref bool holdHeading, double dn, double de)
        {
            //max rand is % of anchor distance
            int maxRand = Convert.ToInt32(.75 *((AnchorDistanceKM < 1500) ?  AnchorDistanceKM : 1500));
            Position position = null;
            bool boundryViolation = false;
            int violationCount = 0;

            while (true)
            {
                
                //turn the ship 
                if(boundryViolation)
                {
                    if (violationCount == 1)
                    {
                        dn = dn * -1;
                    }
                    
                    if (violationCount == 2)
                    {
                        de = de * -1;
                    }

                    if(violationCount > 2)
                    {
                        throw new Exception("Too Many Boundry Violations");
                    }


                    boundryViolation = false;
                }else
                {
                    if(!holdHeading)
                    {
                        dn = _rnd.Next(Convert.ToInt32(maxRand * .8), 1500) * _rnd.NegativePositive();
                        de = _rnd.Next(Convert.ToInt32(maxRand * .05)) * _rnd.NegativePositive();
                    }
                }

               
                position = CalculatePositionHelper.Calculate(start.Latitude, start.Longitude, dn, de);
                _log.LogDebug(Vehicle.VehicleId + " " + dn.ToString() + ", " + de.ToString() + " " + position.Latitude.ToString() + ":" + position.Longitude.ToString());

                if (InBoundaries(position))
                {
                    break;
                }
                else
                {
                    violationCount++;
                    boundryViolation = true;
                    holdHeading = true;
                    if(!(position.DistanceTo(Anchor) <= AnchorDistanceKM))
                    {
                        _log.LogDebug($"{Vehicle.VehicleId} x too far from anchor {position.DistanceTo(Anchor).ToString()}");
                    }
                    else
                    {
                        _log.LogDebug($"{Vehicle.VehicleId} x Not in boundary:" + position.Latitude.ToString() + ", " + position.Longitude.ToString());
                    }
                    
                }
            }
  
            return position;
        }

        private bool InBoundaries(IPosition position)
        {
            return (position.DistanceTo(Anchor) <= AnchorDistanceKM)
                && (position.IsPointInState(AnchorStates)); 
        }

        private void ProcessPosition(List<IPosition> list, IPosition position)
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
