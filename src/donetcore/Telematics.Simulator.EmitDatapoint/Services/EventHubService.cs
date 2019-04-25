using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Telematics.Simulator.EmitDatapoint.Models;

namespace Telematics.Simulator.EmitDatapoint.Services
{
    public class EventHubService : IEventHubService
    {
        private readonly EventHubClient client;
        public EventHubService(IOptions<AppSettings> configuration)
        {
            client = EventHubClient.CreateFromConnectionString(configuration.Value.ServiceBus.ConnectionString);
        }

        public EventHubClient GetClient()
        {
            return client;
        }

        public async Task SendEvent(dynamic message)
        {
            string serializedString = JsonConvert.SerializeObject(message);
            var eventData = new EventData(Encoding.UTF8.GetBytes(serializedString));
            await client.SendAsync(eventData);
        }
    }
}
