using System;

namespace GenerateLatLon.Interfaces
{
    public interface IPosition : ICoordinates, IDeviceEvent
    {
        double de { get; set; }
        double dn { get; set; }
        double DistanceKM { get; set; }
        double SpeedKM { get; set; }
    }
}