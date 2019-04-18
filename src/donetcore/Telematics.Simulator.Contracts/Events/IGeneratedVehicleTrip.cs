using System;
using System.Collections.Generic;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Contracts.Events
{
    public interface IGeneratedVehicleTrip
    {
        IGenerateTripRequest TripRequest { get; set; }
        IEnumerable<ITripPosition> Positions { get; set; }
    }
}
