using System;
using System.Collections.Generic;

namespace GenerateLatLon.Interfaces
{
    public interface IPositionGenerationService
    {
        IEnumerable<IPosition> Generate(IVehicle vehicle, ICoordinates startingPosition, ICoordinates anchor, 
            DateTime startTime, int positions = 1000, int anchorDistanceKM = 1000, string[] anchorStates = null);
    }
}