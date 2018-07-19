using System;

namespace GenerateLatLon.Interfaces
{
    public interface IPosition : IDeviceEvent
    {
        double de { get; set; }
        double dn { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        double DistanceKM { get; set; }
        double SpeedKM { get; set; }
    }
}