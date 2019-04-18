using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Contracts.Events
{
    public interface IVehiclePositionDispatched
    {
        ITripPosition Position { get; set; }
    }
}
