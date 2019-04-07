using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Core.Services
{
    public delegate void PositionGeneratedHandler(object source, PositionEventArgs e);

    public class PositionEventArgs : EventArgs
    {
        public IPosition Position { get; set; }
    }

    public delegate void TripGeneratedHandler(object source, TripEventArgs e);

    public class TripEventArgs
    {
        public IEnumerable<IPosition> Positions { get; set; }

        public IGenerateTripRequest TripRequest { get; set; }
    }

    public class GenerateTripService : IGenerateTripService
    {
        private readonly ILogger _log;
        private readonly IPositionGenerationService _positionGenerationService;

        public event PositionGeneratedHandler PositionGenerated;
        public event TripGeneratedHandler TripGenerated;

        public GenerateTripService(IPositionGenerationService positionGenerationService, ILoggerFactory loggerFactory)
        {
            _positionGenerationService = positionGenerationService;
            _log = loggerFactory.CreateLogger<GenerateTripService>();
        }

        public void GenerateTrips(IGenerateTripRequest tripRequest, int numberOfTripsToGenerate)
        {

            for (int i = 0; i < numberOfTripsToGenerate; i++)
            {
                var positions = _positionGenerationService.Generate(tripRequest);
                var last = positions.LastOrDefault();
                tripRequest.StartTime = last.UtcPositionTime.AddHours(1);
                tripRequest.StartingPosition = last;
                foreach (var result in positions)
                {
                    _log.LogDebug(
                         " type:" + ((result is IBehaviorEvent) ? ((IBehaviorEvent)result).Label : "Position")
                        + " vehicle:" + result.VehicleId
                        + " distance km:" + result.DistanceKM.ToString()
                        + " speed kph:" + result.SpeedKM.ToString());

                    SendTelemetryEvent(result);
                }

                SendTripEvent(positions, tripRequest);
            }
        }

        private void SendTripEvent(IEnumerable<IPosition> positions, IGenerateTripRequest request)
        {
            TripGenerated?.Invoke(this, new TripEventArgs { Positions = positions, TripRequest = request });
        }

        private void SendTelemetryEvent(IPosition result)
        {
            PositionGenerated?.Invoke(this, new PositionEventArgs { Position = result });
        }
    }
}
