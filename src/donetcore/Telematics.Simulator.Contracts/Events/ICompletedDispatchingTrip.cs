using System;
using System.Collections.Generic;
using System.Text;
using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Contracts.Events
{
    public interface ICompletedDispatchingTrip
    {
        string VehicleId { get; set; }

        IPosition LastPosition { get; set; }

        IGenerateTripRequest TripRequest { get; set; }
    }
}
