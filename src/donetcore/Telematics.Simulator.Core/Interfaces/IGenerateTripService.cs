using Telematics.Simulator.Core.Services;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Core.Interfaces
{
    public interface IGenerateTripService
    {
        event TripGeneratedHandler TripGenerated;

        void GenerateTrips(IGenerateTripRequest tripRequest, int numberOfTripsToGenerate);
    }
}