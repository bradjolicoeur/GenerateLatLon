using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Telematics.Simulator.Contracts.Commands;
using Telematics.Simulator.Contracts.Events;

namespace Telematics.Simulator.VehicleSaga
{
    public class VehicleSagaHandler : Saga<VehicleSagaData>, 
        IAmStartedByMessages<InitializeVehicle>,
        IHandleMessages<IGeneratedVehicleTrip>
    {
        private readonly ILogger _log;

        public VehicleSagaHandler(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<VehicleSagaHandler>();
        }

        public Task Handle(InitializeVehicle message, IMessageHandlerContext context)
        {
            _log.LogInformation("Initialize Vehicle " + message.Vehicle.VehicleId);

            Data.InitialPosition = message.StartingPosition;
            Data.LastPosition = message.StartingPosition;
            Data.StartTime = message.StartTime;
            Data.Vehicle = message.Vehicle;
            Data.Anchor = message.Anchor;
            Data.AnchorDistanceKM = message.AnchorDistanceKM;
            Data.AnchorStates = message.AnchorStates;

            return context.Send<GenerateVehicleTrip>(m =>
            {
                m.Vehicle = Data.Vehicle;
                m.Anchor = Data.Anchor;
                m.AnchorDistanceKM = Data.AnchorDistanceKM;
                m.AnchorStates = Data.AnchorStates;
                m.StartTime = Data.StartTime;
                m.StartingPosition = Data.LastPosition;
            });
        }

        public Task Handle(IGeneratedVehicleTrip message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<VehicleSagaData> mapper)
        {
            mapper.ConfigureMapping<InitializeVehicle>(message => message.Vehicle.VehicleId)
             .ToSaga(sagaData => sagaData.VehicleId);

            mapper.ConfigureMapping<IGeneratedVehicleTrip>(message => message.TripRequest.Vehicle.VehicleId)
             .ToSaga(sagaData => sagaData.VehicleId);
        }
    }
}
