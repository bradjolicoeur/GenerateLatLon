using System;

namespace Telematics.Simulator.Models.Interfaces
{
    public interface ITripPosition : IPosition
    {
        string Label { get; set; }
    }
}