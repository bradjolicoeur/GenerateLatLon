using GenerateLatLon.Models;
using System;

namespace GenerateLatLonFunction.Models
{
    public class GenerateTripRequest
    {
        public Vehicle Vehicle { get; set; }
        public string[] AnchorStates { get; set; }
        public int? AnchorDistanceKM { get; set; }
        public int? TripPositions { get; set; }
        public DateTime StartTime { get; set; }
        public Coordinates StartCoordinates { get; set; }
        public Coordinates AnchorCoordinates { get; set; }
    }
}

