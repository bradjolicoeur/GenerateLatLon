using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using Telematics.Simulator.Core.Interfaces;
using Telematics.Simulator.VehicleSaga.Models;

namespace Telematics.Simulator.VehicleSaga
{
    public class VehicleSagaData : ContainSagaData
    {
        public string VehicleId { get; set; }

        public IVehicle Vehicle { get; set; }

        public ICoordinates InitialPosition { get; set; }

        public IPosition LastPosition { get; set; }

        public IGenerateTripRequest LastTripRequest { get; set; }

        public ICoordinates Anchor { get; set; }

        public int AnchorDistanceKM { get; set; } = 1000;

        public string[] AnchorStates { get; set; }

        public DateTime NextTripStartTime { get; set; }
    }
}
