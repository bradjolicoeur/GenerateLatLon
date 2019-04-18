using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Core.Models
{
    public abstract class BaseBehaviorEvent : Position, IBehaviorEvent
    {
        public BaseBehaviorEvent(IPosition position, string label)
        {
            Label = label;
            Latitude = position.Latitude;
            Longitude = position.Longitude;
            dn = position.dn;
            de = position.de;
            UtcPositionTime = position.UtcPositionTime;
            DistanceKM = position.DistanceKM;
            SpeedKM = position.SpeedKM;
        }

        public string Label { get; private set; }
    }
}
