using GenerateLatLon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateLatLon.Helpers
{
    public static class GeoFunctions
    {
        public static double Speed(double distanceKM, DateTime start, DateTime end)
        {
            double timeHrs = start.Subtract(end).TotalHours;

            return distanceKM / timeHrs;
        }

        public static double DistanceTo(this Coordinates start, Coordinates end, char unit = 'K')
        {
            return DistanceTo(start.Latitude, start.Longitude, end.Latitude, end.Longitude, unit);
        }

        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        public static bool IsPointInPolygon(List<Coordinates> poly, Coordinates point)
        {
            int i, j;
            bool condition = false;
            for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                if ((((poly[i].Longitude <= point.Latitude) && (point.Latitude < poly[j].Latitude))
                        || ((poly[j].Latitude <= point.Latitude) && (point.Latitude < poly[i].Latitude)))
                        && (point.Longitude < (poly[j].Longitude - poly[i].Longitude) * (point.Latitude - poly[i].Latitude)
                            / (poly[j].Latitude - poly[i].Latitude) + poly[i].Longitude))

                    condition = !condition;
            }

            return condition;
        }

    }
}
