using System;
using System.Collections.Generic;
using System.Text;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.VehicleSaga.Models
{
    public class VehicleTrip
    {
        public IGenerateTripRequest TripRequest { get; set; }
        public IEnumerable<IPosition> Positions { get; set; }
    }
}
