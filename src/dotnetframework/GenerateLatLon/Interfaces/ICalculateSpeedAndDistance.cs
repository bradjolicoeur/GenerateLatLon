
namespace GenerateLatLon.Interfaces
{
    public interface ICalculateSpeedAndDistance
    {
        IPosition Calulate(IPosition position, IPosition previousPosition);
    }
}