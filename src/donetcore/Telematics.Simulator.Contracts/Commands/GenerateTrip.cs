using System;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Contracts.Commands
{
    public class GenerateVehicleTrip
    {
        public ICoordinates Anchor { get; set; }
        public int AnchorDistanceKM { get; set; }
        public string[] AnchorStates { get; set; }
        public ICoordinates StartingPosition { get; set; }
        public DateTime StartTime { get; set; }
        public IVehicle Vehicle { get; set; }
    }
}
