using GenerateLatLon.Interfaces;
using System;

namespace GenerateLatLon.Models
{
    public class Position : IPosition
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double dn { get; set; }
        public double de { get; set; }
        public DateTime UtcPositionTime { get; set; }
    }
}
