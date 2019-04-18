using System;
using System.Collections.Generic;
using System.Text;

using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Contracts.Models
{
    public class TripPosition : ITripPosition
    {
        public double DistanceKM { get; set; }
        public double SpeedKM { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime UtcPositionTime { get; set; }
        public string VehicleId { get; set; }
        public string Label { get; set; }
        public double de { get; set; }
        public double dn { get; set; }
    }
}
