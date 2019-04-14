using System;
using System.Collections.Generic;
using System.Text;
using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Contracts.Events
{
    public interface IGeneratedVehicleTrip
    {
        IGenerateTripRequest TripRequest { get; set; }
        IEnumerable<IPosition> Positions { get; set; }
    }
}
