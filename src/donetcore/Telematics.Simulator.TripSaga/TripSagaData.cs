using NServiceBus;
using System;
using System.Collections.Generic;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.TripSaga
{
    public class TripSagaData : ContainSagaData
    {
        public string VehicleId { get; set; }
        public IGenerateTripRequest TripRequest { get; set; }
        public IList<ITripPosition> Positions { get; set; }
        public IList<DateTime> PositionTimes { get; set; }
        public int LastPoint { get; set; }
    }
}
