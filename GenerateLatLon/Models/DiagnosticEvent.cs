using GenerateLatLon.Interfaces;
using System;

namespace GenerateLatLon.Models
{
    public class DiagnosticEvent : BaseBehaviorEvent, IBehaviorEvent, IPosition, IDeviceEvent
    {
        public DiagnosticEvent(IPosition position) : base(position, "Diagnostic")
        {
        }
    }
}
