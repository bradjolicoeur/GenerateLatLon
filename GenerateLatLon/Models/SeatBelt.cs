using GenerateLatLon.Interfaces;

namespace GenerateLatLon.Models
{
    public class Seatbelt : BaseBehaviorEvent, IBehaviorEvent, IDeviceEvent
    {
        public Seatbelt(IPosition position) : base(position, "Seatbelt")
        {
        }
    }
}
