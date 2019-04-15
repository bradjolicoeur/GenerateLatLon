using NServiceBus;
using System.Collections.Generic;
using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.TripSaga
{
    public class TripSagaData : ContainSagaData
    {
        public string VehicleId { get; set; }
        public IGenerateTripRequest TripRequest { get; set; }
        public IList<IPosition> Positions { get; set; }
        public int LastPoint { get; set; }
    }
}
