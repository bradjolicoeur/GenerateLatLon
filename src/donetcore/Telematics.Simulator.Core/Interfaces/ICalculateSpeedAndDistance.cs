
namespace Telematics.Simulator.Core.Interfaces
{
    public interface ICalculateSpeedAndDistance
    {
        IPosition Calulate(IPosition position, IPosition previousPosition);
    }
}