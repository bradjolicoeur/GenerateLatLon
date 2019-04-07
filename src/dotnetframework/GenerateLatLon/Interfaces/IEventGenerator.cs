using System.Collections.Generic;
using GenerateLatLon.Interfaces;

namespace GenerateLatLon
{
    public interface IEventGenerator
    {
        IEnumerable<IPosition> Generate(IPosition position);
    }
}