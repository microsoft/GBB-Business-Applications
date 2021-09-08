using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Azure.Messaging.ServiceBus;

namespace CaseAPI.Model
{
    class ServiceBus
    {
        string connectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
        string queueName = Environment.GetEnvironmentVariable("ServiceBusQueueName");

        public async Task SendAsync(dynamic message) {

            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(queueName);

            //Add Logic to Checck if already exists

            ServiceBusMessage sbMessage = new ServiceBusMessage(message.ToString());

            await sender.SendMessageAsync(sbMessage);


        }
    }
}
