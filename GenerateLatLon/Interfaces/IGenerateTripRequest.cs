using System;

namespace GenerateLatLon.Interfaces
{
    public interface IGenerateTripRequest
    {
        ICoordinates Anchor { get; set; }
        int AnchorDistanceKM { get; set; }
        string[] AnchorStates { get; set; }
        int NumberOfPositions { get; set; }
        ICoordinates StartingPosition { get; set; }
        DateTime StartTime { get; set; }
        IVehicle Vehicle { get; set; }
    }
}