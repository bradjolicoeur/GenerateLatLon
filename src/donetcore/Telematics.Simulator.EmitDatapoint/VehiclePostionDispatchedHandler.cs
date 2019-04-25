using Telematics.Simulator.Contracts.Events;
using NServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telematics.Simulator.EmitDatapoint.Services;


namespace Telematics.Simulator.EmitDatapoint
{
    public class VehiclePostionDispatchedHandler : IHandleMessages<IVehiclePositionDispatched>
    {
        private readonly ILogger _log;
        private readonly IEventHubService _hubClient;

        public VehiclePostionDispatchedHandler(ILoggerFactory loggerFactory, IEventHubService hubFactory)
        {
            _log = loggerFactory.CreateLogger<VehiclePostionDispatchedHandler>();
            _hubClient = hubFactory;
        }

        public Task Handle(IVehiclePositionDispatched message, IMessageHandlerContext context)
        {
            //TODO: for now we are just going to log this
            _log.LogInformation("Position Dispatched for " + message.Position.VehicleId + " " + message.Position.UtcPositionTime.ToString() + " " + message.Position.Label);

            _hubClient.SendEvent(message.Position);

            return Task.CompletedTask;
        }
    }
}
