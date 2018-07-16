using GenerateLatLon.Interfaces;

namespace GenerateLatLon.Models
{
    public abstract class BaseBehaviorEvent : Position, IBehaviorEvent
    {
        public BaseBehaviorEvent(IPosition position, string label)
        {
            Label = label;
            latitude = position.latitude;
            longitude = position.longitude;
            dn = position.dn;
            de = position.de;
            UtcPositionTime = position.UtcPositionTime;
        }

        public string Label { get; private set; }
    }
}
