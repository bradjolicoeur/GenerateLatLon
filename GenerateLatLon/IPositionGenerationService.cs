using System;
using System.Collections.Generic;
using GenerateLatLon.Interfaces;

namespace GenerateLatLon
{
    public interface IPositionGenerationService
    {
        IEnumerable<IPosition> Generate(ICoordinates startingPosition, ICoordinates anchor, 
            DateTime startTime, int positions = 1000, int anchorDistanceKM = 1000, string[] anchorStates = null);
    }
}