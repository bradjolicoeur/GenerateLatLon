using GenerateLatLon.Interfaces;

namespace GenerateLatLon.Models
{
    public class HardBraking : BaseBehaviorEvent, IBehaviorEvent, IPosition, IDeviceEvent
    {
        public HardBraking(IPosition position) : base(position, "HardBraking")
        {
        }
    }
}
