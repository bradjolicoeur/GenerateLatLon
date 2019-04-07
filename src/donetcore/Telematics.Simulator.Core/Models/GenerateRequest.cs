using Telematics.Simulator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telematics.Simulator.Core.Models
{
    public class GenerateTripRequest : IGenerateTripRequest
    {
        public IVehicle Vehicle { get; set; }

        public ICoordinates StartingPosition { get; set; }

        public DateTime StartTime { get; set; }

        public int NumberOfPositions { get; set; } = 500;

        public ICoordinates Anchor { get; set; }

        public int AnchorDistanceKM { get; set; } = 1000;

        public string[] AnchorStates { get; set; }

    }
}
