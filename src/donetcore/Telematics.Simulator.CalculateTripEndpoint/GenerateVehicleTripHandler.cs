using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Telematics.Simulator.Contracts.Commands;
using Telematics.Simulator.Contracts.Events;
using Telematics.Simulator.Contracts.Models;
using Telematics.Simulator.Core.Interfaces;
using Telematics.Simulator.Core.Models;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.CalculateTripEndpoint
{
    public class GenerateVehicleTripHandler : IHandleMessages<GenerateVehicleTrip>
    {
        private readonly IPositionGenerationService _generateTripSerice;
        private readonly ILogger _log;
        private readonly Random _rnd;

        public GenerateVehicleTripHandler(IPositionGenerationService generateTripService, 
                                            ILoggerFactory loggerFactory,
                                            IRandomFactory randomFactory)
        {
            _generateTripSerice = generateTripService;
            _rnd = randomFactory.Create();
            _log = loggerFactory.CreateLogger<GenerateVehicleTripHandler>();

        }

        public Task Handle(GenerateVehicleTrip message, IMessageHandlerContext context)
        {
            _log.LogInformation("Calculating Trip for " + message.Vehicle.VehicleId);

            var request = new GenerateTripRequest
            {
                StartingPosition = message.StartingPosition,
                Anchor = message.Anchor,
                AnchorDistanceKM = message.AnchorDistanceKM,
                AnchorStates = message.AnchorStates,
                StartTime = message.StartTime,
                Vehicle = message.Vehicle,
                NumberOfPositions = 5 //_rnd.Next(100, 300)
            };

            var result = _generateTripSerice.Generate(request);

            return context.Publish<IGeneratedVehicleTrip>(
                messageConstructor: m =>
            {
                m.TripRequest = request;
                m.Positions = ConvertPositions(result);
            });
        }

        public IEnumerable<ITripPosition> ConvertPositions(IEnumerable<IPosition> positions)
        {
            var tripPositions = new List<ITripPosition>();
            foreach (var position in positions)
            {

                string label = null;
                if(position is IBehaviorEvent be)
                {
                    label = be.Label;
                }

                tripPositions.Add(new TripPosition
                {
                    DistanceKM = position.DistanceKM,
                    SpeedKM = position.SpeedKM,
                    Latitude = position.Latitude,
                    Longitude = position.Longitude,
                    UtcPositionTime = position.UtcPositionTime,
                    VehicleId = position.VehicleId,
                    Label = label
                });
            }

            return tripPositions;
        }
    }
}
