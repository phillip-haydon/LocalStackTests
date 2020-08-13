using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.CloudWatchEvents;
using Amazon.Lambda.Core;
using Amazon.SQS;
using Amazon.SQS.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.CamelCaseLambdaJsonSerializer))]

namespace TestLambdas
{
    public class InputEvent
    {
        public string Value { get; set; }
    }
    
    // public class RaiseSQSFunction
    // {
    //     public async Task<string> Handler(InputEvent input, ILambdaContext context)
    //     {
    //         var host = Environment.GetEnvironmentVariable("LOCALSTACK_HOSTNAME") ?? "localhost";
    //         
    //         Console.WriteLine(host);
    //         
    //         var sqsClient = new AmazonSQSClient(new AmazonSQSConfig
    //         {
    //             ServiceURL = $"http://{host}:4566",
    //             UseHttp = true
    //         });
    //
    //         var queueUrl = await sqsClient.GetQueueUrlAsync("banana", default);
    //
    //         var result = await sqsClient.SendMessageAsync(new SendMessageRequest
    //         {
    //             QueueUrl = queueUrl.QueueUrl,
    //             MessageBody = JsonSerializer.Serialize(input)
    //         });
    //         
    //         return result.HttpStatusCode == HttpStatusCode.OK ? "Success" : "Failure";
    //     }
    // }

    public class TriggerFromEventBridge
    {
        public async Task<bool> HandlerEvent(CloudWatchEvent<InputEvent> @event, ILambdaContext context)
        {
            Console.WriteLine(JsonSerializer.Serialize(@event));

            var client = new HttpClient();

            await client.PostAsync("https://webhook.site/4173f469-44ef-473d-a753-c8c8d4a994bc", new StringContent("Message from `CloudWatchEvent` version: " + JsonSerializer.Serialize(@event)));

            return true;
        }
        
        public async Task<bool> HandlerObject(object @event, ILambdaContext context)
        {
            Console.WriteLine(JsonSerializer.Serialize(@event));

            var client = new HttpClient();

            await client.PostAsync("https://webhook.site/4173f469-44ef-473d-a753-c8c8d4a994bc", new StringContent("Message from `object` version: " + JsonSerializer.Serialize(@event)));

            return true;
        }
    }
}