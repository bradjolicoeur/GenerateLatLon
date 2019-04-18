using System.Collections.Generic;
using Telematics.Simulator.Core.Interfaces;
using Telematics.Simulator.Models.Interfaces;

namespace Telematics.Simulator.Core.Interfaces
{
    public interface IEventGenerator
    {
        IEnumerable<IPosition> Generate(IPosition position);
    }
}