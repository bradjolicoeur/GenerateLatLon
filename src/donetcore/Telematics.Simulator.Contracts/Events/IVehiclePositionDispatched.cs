using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Contracts.Events
{
    public interface IVehiclePositionDispatched
    {
        IPosition Position { get; set; }
    }
}
