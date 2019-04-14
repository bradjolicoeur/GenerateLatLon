using Telematics.Simulator.Core.Services;

namespace Telematics.Simulator.Core.Interfaces
{
    public interface IGenerateTripService
    {
        event TripGeneratedHandler TripGenerated;

        void GenerateTrips(IGenerateTripRequest tripRequest, int numberOfTripsToGenerate);
    }
}