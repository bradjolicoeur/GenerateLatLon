using System;

namespace GenerateLatLon.Interfaces
{
    public interface IPosition : IDeviceEvent
    {
        double de { get; set; }
        double dn { get; set; }
        double latitude { get; set; }
        double longitude { get; set; }
    }
}