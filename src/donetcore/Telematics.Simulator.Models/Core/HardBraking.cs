using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Core.Models
{
    public class HardBraking : BaseBehaviorEvent, IBehaviorEvent, IPosition, IDeviceEvent
    {
        public HardBraking(IPosition position) : base(position, "HardBraking")
        {
        }
    }
}
