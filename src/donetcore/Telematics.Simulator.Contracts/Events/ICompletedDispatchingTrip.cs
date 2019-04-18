using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Contracts.Events
{
    public interface ICompletedDispatchingTrip
    {
        string VehicleId { get; set; }

        ITripPosition LastPosition { get; set; }

        IGenerateTripRequest TripRequest { get; set; }
    }
}
