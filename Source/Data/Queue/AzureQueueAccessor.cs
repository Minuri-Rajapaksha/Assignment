using Data.Interfaces.Queue;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Data.Queue
{
    public class AzureQueueAccessor<T> : IQueueAccessor<T> where T : class
    {
        private readonly QueueClient _client;

        public AzureQueueAccessor(IConfiguration configuration)
        {
            _client = new QueueClient(configuration.GetConnectionString("ServiceBus"), this.GetType().GetGenericArguments()[0].Name.ToLowerInvariant());
        }

        public async Task SendAsync(T item)
        {
            await _client.SendAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item))));
        }

        public async Task Receive(Func<T, Task> onProcess)
        {
            var options = new MessageHandlerOptions(e =>
            {
                return Task.CompletedTask;
            })
            {
                AutoComplete = false,
                MaxAutoRenewDuration = TimeSpan.FromMinutes(1)
            };

            _client.RegisterMessageHandler(
                async (message, token) =>
                {
                    try
                    {
                        // Get message  
                        var data = Encoding.UTF8.GetString(message.Body);
                        T item = JsonConvert.DeserializeObject<T>(data);

                        // Process message  
                        await onProcess(item);
                        await _client.CompleteAsync(message.SystemProperties.LockToken);
                    }
                    catch (Exception)
                    {
                        await _client.DeadLetterAsync(message.SystemProperties.LockToken);
                    }
                }, options);
        }
    }
}
