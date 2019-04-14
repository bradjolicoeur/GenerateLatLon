using System;
using System.Collections.Generic;
using System.Text;
using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Contracts.Commands
{
    public class InitializeVehicle
    {
        public IVehicle Vehicle { get; set; }

        public ICoordinates StartingPosition { get; set; }

        public DateTime StartTime { get; set; }

        public ICoordinates Anchor { get; set; }

        public int AnchorDistanceKM { get; set; } = 1000;

        public string[] AnchorStates { get; set; }
    }
}
