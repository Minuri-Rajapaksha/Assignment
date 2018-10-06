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

        public void Receive(
            Func<T, Shared.Enum.Queue> onProcess,
            Action<Exception> onError,
            Action onWait)
        {
            var options = new MessageHandlerOptions(e =>
            {
                onError(e.Exception);
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
                        var result = onProcess(item);

                        if (result == Shared.Enum.Queue.Complete)
                            await _client.CompleteAsync(message.SystemProperties.LockToken);
                        else if (result == Shared.Enum.Queue.Abandon)
                            await _client.AbandonAsync(message.SystemProperties.LockToken);
                        else if (result == Shared.Enum.Queue.Dead)
                            await _client.DeadLetterAsync(message.SystemProperties.LockToken);

                        // Wait for next message  
                        onWait();
                    }
                    catch (Exception ex)
                    {
                        await _client.DeadLetterAsync(message.SystemProperties.LockToken);
                        onError(ex);
                    }
                }, options);
        }
    }
}
