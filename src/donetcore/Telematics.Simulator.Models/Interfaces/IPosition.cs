using System;

namespace Telematics.Simulator.Models.Interfaces
{
    public interface IPosition : ICoordinates, IDeviceEvent, IVehicle
    {
        double de { get; set; }
        double dn { get; set; }
        double DistanceKM { get; set; }
        double SpeedKM { get; set; }
    }
}