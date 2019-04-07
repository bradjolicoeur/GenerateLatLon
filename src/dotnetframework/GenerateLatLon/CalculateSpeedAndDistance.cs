using GenerateLatLon.Interfaces;
using GenerateLatLon.Helpers;
using System;

namespace GenerateLatLon
{
    public class CalculateSpeedAndDistance : ICalculateSpeedAndDistance
    {

        public IPosition Calulate(IPosition position, IPosition previousPosition)
        {
            if (previousPosition != null)
            { 
                position.DistanceKM = Math.Round(
                    GeoFunctions.DistanceTo(position.Latitude, position.Longitude, previousPosition.Latitude, previousPosition.Longitude), 2);
                position.SpeedKM = Math.Round(
                    GeoFunctions.Speed(position.DistanceKM, position.UtcPositionTime, previousPosition.UtcPositionTime),2);
            }

            return position;
        }


    }
}
