using Telematics.Simulator.Core.Interfaces;


namespace Telematics.Simulator.Core.Models
{
    public class HardAcceleration : BaseBehaviorEvent, IBehaviorEvent, IPosition, IDeviceEvent
    {
        public HardAcceleration(IPosition position) : base(position, "HardAcceleration")
        {
        }
    }
}
