
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using GenerateLatLonFunction.Models;
using GenerateLatLon.Interfaces;
using GenerateLatLon;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GenerateLatLonFunction
{
    public static class GenerateTrip
    {


        [FunctionName("GenerateTrip")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            var request = JsonConvert.DeserializeObject<GenerateTripRequest>(content);

            IPositionGenerationService service = new PositionGenerationService(new EventGenerator(), new CalculateSpeedAndDistance());

            var positions = service.Generate(request.Vehicle, request.StartCoordinates, 
                request.AnchorCoordinates ?? request.StartCoordinates, request.StartTime,
                request.TripPositions ?? 500, request.AnchorDistanceKM ?? 1000, request.AnchorStates);

            return positions != null
                //? (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(new FunctionResponse { Success = true, Rows = positions.Count() }))
                ? (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(positions))
                : new BadRequestObjectResult(JsonConvert.SerializeObject(new FunctionResponse { Success = false, Message = "request was unsuccessful" }));
        }

       
    }
}
