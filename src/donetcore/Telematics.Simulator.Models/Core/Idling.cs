using Telematics.Simulator.Core.Interfaces;
using System;

namespace Telematics.Simulator.Core.Models
{
    public class Idling : BaseBehaviorEvent, IBehaviorEvent, IDeviceEvent
    {
        public Idling(IPosition position) : base(position, "Idling")
        {
            SpeedKM = 0;
            DistanceKM = 0;
        }
    }
}
