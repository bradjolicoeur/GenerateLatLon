using GenerateLatLon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLatLon
{
    public static class CalculatePositionHelper
    {
        /// <summary>
        /// Method to calculate a new position based on meters
        /// </summary>
        /// <param name="lat">Starting latitude</param>
        /// <param name="lon">Starting longitude</param>
        /// <param name="dn">meters added to latitude</param>
        /// <param name="de">meters added to longitude</param>
        /// <returns></returns>
        public static Position Calculate(double lat, double lon, double dn, double de)
        {
            //Earth’s radius, sphere
            int R = 6378137;

            //Coordinate offsets in radians
            var dLat = dn / R;
            var dLon = de / (R * Math.Cos(Math.PI * lat / 180));

            //OffsetPosition, decimal degrees
            return new Position
            {
                Latitude = Math.Round(lat + dLat * 180 / Math.PI, 6),
                Longitude = Math.Round(lon + dLon * 180 / Math.PI, 6),
                dn = dn,
                de = de
            };
        }

    }
}
