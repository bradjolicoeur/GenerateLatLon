using System;
using System.Collections.Generic;
using System.Text;
using Telematics.Simulator.Contracts.Events;
using NServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.EmitDatapoint
{
    public class VehiclePostionDispatchedHandler : IHandleMessages<IVehiclePositionDispatched>
    {
        private readonly ILogger _log;

        public VehiclePostionDispatchedHandler(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<VehiclePostionDispatchedHandler>();
        }

        public Task Handle(IVehiclePositionDispatched message, IMessageHandlerContext context)
        {
            //TODO: for now we are just going to log this
            _log.LogInformation("Position Dispatched for " + message.Position.VehicleId + " " + message.Position.UtcPositionTime.ToString() + " " + message.Position.Label);

            return Task.CompletedTask;
        }
    }
}
