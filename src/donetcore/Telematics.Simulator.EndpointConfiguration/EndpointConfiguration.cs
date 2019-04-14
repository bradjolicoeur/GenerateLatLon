using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using Telematics.Simulator.Contracts.Commands;
using Telematics.Simulator.Contracts.Events;
using Telematics.Simulator.EndpointConfiguration;

namespace Telematics.Simulator.EndpointConf
{
    public static class EndpointConfigurations
    {

        public static NServiceBus.EndpointConfiguration ConfigureNSB(ServiceCollection serviceCollection, string endpointName)
        {

            var endpointConfiguration = new NServiceBus.EndpointConfiguration(endpointName);

            //configuring audit queue and error queue
            endpointConfiguration.AuditProcessedMessagesTo("audit"); //copy of message after processing will go here for servicecontroller
            endpointConfiguration.SendFailedMessagesTo("error"); //after specified retries is hit, message will be moved here for alerting and recovery

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.NoPayloadSizeRestriction(); //TODO:need to implement databus

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(GenerateVehicleTrip), "CalculateTripEndpoint");
            routing.RouteToEndpoint(typeof(InitializeVehicle), "VehicleSagaEndpoint");
            //routing.RegisterPublisher(typeof(IGeneratedVehicleTrip), "CalculateTripEndpoint");

            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var conventions = endpointConfiguration.Conventions();
            NSBConventions.ConfigureConventions(conventions);

            endpointConfiguration.EnableInstallers(); 

            endpointConfiguration.UseContainer<ServicesBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingServices(serviceCollection);
            });

            return endpointConfiguration;

        }
    }
}
