using GenerateLatLon.Interfaces;
using System;

namespace GenerateLatLon.Models
{
    public class Idling : BaseBehaviorEvent, IBehaviorEvent, IDeviceEvent
    {
        public Idling(IPosition position) : base(position, "Idling")
        {
        }
    }
}
