using GenerateLatLon.Interfaces;

namespace GenerateLatLon.Models
{
    public class Coordinates : ICoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinates() { }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

    }
}
