using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Contracts.Commands
{
    public class StreamVehicleTrip
    {
          public IGenerateTripRequest TripRequest { get; set; }
    }
}
