using Microsoft.Azure.EventHubs;
using System.Threading.Tasks;

namespace Telematics.Simulator.EmitDatapoint.Services
{
    public interface IEventHubService
    {
        EventHubClient GetClient();
        Task SendEvent(dynamic message);
    }
}