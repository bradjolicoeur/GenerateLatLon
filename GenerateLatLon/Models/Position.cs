using GenerateLatLon.Interfaces;
using System;

namespace GenerateLatLon.Models
{
    public class Position : IPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double dn { get; set; }
        public double de { get; set; }
        public DateTime UtcPositionTime { get; set; }
        public double DistanceKM { get; set; }
        public double SpeedKM { get; set; }
    }
}
