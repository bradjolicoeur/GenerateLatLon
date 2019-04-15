using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Telematics.Simulator.Contracts.Events;

namespace Telematics.Simulator.EmitDatapoint
{
    public class VehicleHeartbeatHandler : IHandleMessages<IVehicleHeartbeat>
    {
        private readonly ILogger _log;

        public VehicleHeartbeatHandler(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<VehicleHeartbeatHandler>();
        }

        public Task Handle(IVehicleHeartbeat message, IMessageHandlerContext context)
        {
            //TODO: for now we are just going to log this
            _log.LogInformation("Heartbeat Dispatched for " + message.Position.VehicleId);

            return Task.CompletedTask;
        }
    }
}
