using GenerateLatLon.Interfaces;


namespace GenerateLatLon.Models
{
    public class HardAcceleration : BaseBehaviorEvent, IBehaviorEvent, IPosition, IDeviceEvent
    {
        public HardAcceleration(IPosition position) : base(position, "HardAcceleration")
        {
        }
    }
}
