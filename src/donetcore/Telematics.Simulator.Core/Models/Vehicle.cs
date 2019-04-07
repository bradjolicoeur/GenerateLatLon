using Telematics.Simulator.Core.Interfaces;

namespace Telematics.Simulator.Core.Models
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
