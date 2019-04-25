using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Telematics.Simulator.Contracts.Events;
using Telematics.Simulator.EmitDatapoint.Services;

namespace Telematics.Simulator.EmitDatapoint
{
    public class VehicleHeartbeatHandler : IHandleMessages<IVehicleHeartbeat>
    {
        private readonly ILogger _log;
        private readonly IEventHubService _hubClient;

        public VehicleHeartbeatHandler(ILoggerFactory loggerFactory, IEventHubService hubFactory)
        {
            _log = loggerFactory.CreateLogger<VehicleHeartbeatHandler>();
            _hubClient = hubFactory;
        }

        public Task Handle(IVehicleHeartbeat message, IMessageHandlerContext context)
        {
            //TODO: for now we are just going to log this
            _log.LogInformation("Heartbeat Dispatched for " + message.Position.VehicleId);

            _hubClient.SendEvent(message.Position);

            return Task.CompletedTask;
        }
    }
}
