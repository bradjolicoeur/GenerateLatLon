using Telematics.Simulator.Models.Interfaces;
using System;

namespace Telematics.Simulator.Core.Models
{
    public class DiagnosticEvent : BaseBehaviorEvent, IBehaviorEvent, IPosition, IDeviceEvent
    {
        public DiagnosticEvent(IPosition position) : base(position, "Diagnostic")
        {
        }
    }
}
