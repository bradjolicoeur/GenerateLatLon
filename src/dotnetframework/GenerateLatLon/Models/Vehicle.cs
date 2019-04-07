using GenerateLatLon.Interfaces;

namespace GenerateLatLon.Models
{
    public class Vehicle : IVehicle
    {
        public Vehicle() { }
        public Vehicle(string vehicleId)
        {
            VehicleId = vehicleId;
        }
        public string VehicleId { get; set; }
    }
}
