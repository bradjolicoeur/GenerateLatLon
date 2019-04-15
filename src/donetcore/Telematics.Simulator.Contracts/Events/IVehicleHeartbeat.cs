using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Contracts.Events
{
    public interface IVehicleHeartbeat
    {
        IPosition Position { get; set; }
    }
}
