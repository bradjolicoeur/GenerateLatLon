using Telematics.Simulator.Models.Interfaces;


namespace Telematics.Simulator.Core.Models
{
    public class HardAcceleration : BaseBehaviorEvent, IBehaviorEvent, IPosition, IDeviceEvent
    {
        public HardAcceleration(IPosition position) : base(position, "HardAcceleration")
        {
        }
    }
}
