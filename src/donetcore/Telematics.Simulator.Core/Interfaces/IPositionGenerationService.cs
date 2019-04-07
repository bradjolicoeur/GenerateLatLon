using System;
using System.Collections.Generic;

namespace Telematics.Simulator.Core.Interfaces
{
    public interface IPositionGenerationService
    {
        IEnumerable<IPosition> Generate(IVehicle vehicle, ICoordinates startingPosition, ICoordinates anchor, 
            DateTime startTime, int positions = 500, int anchorDistanceKM = 1000, string[] anchorStates = null);

        IEnumerable<IPosition> Generate(IGenerateTripRequest tripRequest);
    }
}