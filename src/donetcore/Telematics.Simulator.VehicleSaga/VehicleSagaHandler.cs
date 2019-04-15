using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Telematics.Simulator.Contracts.Commands;
using Telematics.Simulator.Contracts.Events;
using Telematics.Simulator.Core.Models;

namespace Telematics.Simulator.VehicleSaga
{
    public class VehicleSagaHandler : Saga<VehicleSagaData>, 
        IAmStartedByMessages<InitializeVehicle>,
        IHandleMessages<ICompletedDispatchingTrip>,
        IHandleTimeouts<EmitHeartbeatTimeout>
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
                m.StartTime = message.StartTime;
                m.StartingPosition = Data.InitialPosition;
            });
        }

        public async Task Handle(ICompletedDispatchingTrip message, IMessageHandlerContext context)
        {

            Data.LastPosition = message.LastPosition;
            Data.LastTripRequest = message.TripRequest;
            Data.NextTripStartTime = Data.LastPosition.UtcPositionTime.AddHours(1);

            await RequestTimeout<EmitHeartbeatTimeout>(context, TimeSpan.FromMinutes(5), new EmitHeartbeatTimeout { });

            return;
        }

        public async Task Timeout(EmitHeartbeatTimeout state, IMessageHandlerContext context)
        {
            const int heartbeatMinutes = 5;

            Data.LastPosition.UtcPositionTime = Data.LastPosition.UtcPositionTime
                .AddMinutes(state.HeartbeatMinutes);

            await context.Publish<IVehicleHeartbeat>(messageConstructor: m =>
            {
                m.Position = Data.LastPosition;
            });

            if (Data.LastPosition.UtcPositionTime.AddMinutes(heartbeatMinutes) < Data.NextTripStartTime)
            {
                //keep schedule the next heartbeat
                await RequestTimeout<EmitHeartbeatTimeout>(context, TimeSpan.FromMinutes(heartbeatMinutes), 
                    new EmitHeartbeatTimeout { HeartbeatMinutes = heartbeatMinutes });
            } else
            {
                //start the next trip
                await context.Send<GenerateVehicleTrip>(m =>
                {
                    m.Vehicle = Data.Vehicle;
                    m.Anchor = Data.Anchor;
                    m.AnchorDistanceKM = Data.AnchorDistanceKM;
                    m.AnchorStates = Data.AnchorStates;
                    m.StartTime = Data.LastPosition.UtcPositionTime;
                    m.StartingPosition = Data.LastPosition;
                });
            }

        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<VehicleSagaData> mapper)
        {
            mapper.ConfigureMapping<InitializeVehicle>(message => message.Vehicle.VehicleId)
             .ToSaga(sagaData => sagaData.VehicleId);

            mapper.ConfigureMapping<ICompletedDispatchingTrip>(message => message.VehicleId)
             .ToSaga(sagaData => sagaData.VehicleId);
        }

    }
}
