﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Telematics.Simulator.Contracts.Events;
using Telematics.Simulator.TripSaga.Timeouts;

namespace Telematics.Simulator.TripSaga
{
    public class TripSagaHandler : Saga<TripSagaData>
        , IAmStartedByMessages<IGeneratedVehicleTrip>
        , IHandleTimeouts<EmitPositionTimeout>
    {
        private readonly ILogger _log;

        public TripSagaHandler(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<TripSagaHandler>();
        }

        public async Task Handle(IGeneratedVehicleTrip message, IMessageHandlerContext context)
        {
            _log.LogInformation("Initialized dispatch of trip for " + Data.VehicleId);

            Data.TripRequest = message.TripRequest;
            Data.Positions = message.Positions.ToList();

            await RequestTimeout<EmitPositionTimeout>(context, TimeSpan.FromSeconds(1));
        }

        public async Task Timeout(EmitPositionTimeout state, IMessageHandlerContext context)
        {
            _log.LogInformation("Dispatch position for " + Data.VehicleId );

            //TODO: if there are events with the same time stamp they need to be
            //sent as a batch...need to do some grouping here
            //find the current position to dispatch
            var position = Data.Positions.OrderBy(o => o.UtcPositionTime)
                .Skip(Data.LastPoint).Take(1).FirstOrDefault();

            Data.LastPoint++;

            //Dispatch the vehicle position
            await context.Publish<IVehiclePositionDispatched>(messageConstructor: m =>
            {
                m.Position = position;
            });

            //find the next position in the series
            var nextPosition = Data.Positions
                    .Where(p => p.UtcPositionTime > position.UtcPositionTime)
                    .OrderBy(o => o.UtcPositionTime).FirstOrDefault();

            if(nextPosition != null)
            {
                //find the time to raise the next timeout to dispatch a position
                var nextTime = nextPosition.UtcPositionTime.Subtract(position.UtcPositionTime);

                _log.LogInformation(Data.VehicleId + " next position in " + nextTime.ToString());

                //Request the next timeout
                await RequestTimeout<EmitPositionTimeout>(context, nextTime);
            }
            else
            {
                //I completed dispatching trip
                await context.Publish<ICompletedDispatchingTrip>(messageConstructor: m =>
                {
                    m.VehicleId = Data.VehicleId;
                    m.LastPosition = position;
                    m.TripRequest = Data.TripRequest;
                });

                MarkAsComplete();
            }
            
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TripSagaData> mapper)
        {
            mapper.ConfigureMapping<IGeneratedVehicleTrip>(message => message.TripRequest.Vehicle.VehicleId)
            .ToSaga(sagaData => sagaData.VehicleId);
        }
    }
}
