using GenerateLatLon.Interfaces;

namespace GenerateLatLon
{
    public interface ICalculateSpeedAndDistance
    {
        IPosition Calulate(IPosition position, IPosition previousPosition);
    }
}