using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Core.Models
{
    public class Seatbelt : BaseBehaviorEvent, IBehaviorEvent, IDeviceEvent
    {
        public Seatbelt(IPosition position) : base(position, "Seatbelt")
        {
        }
    }
}
