using Data.Queue.Interfaces;
using Microsoft.Azure.ServiceBus;

namespace Data.Queue
{
    public class AzureQueueAccessor<T> : IQueueAccessor<T> where T : class
    {
        private readonly QueueClient _client;

        public AzureQueueAccessor()
        {
            Init();
        }

        private void Init()
        {
            //client = new QueueClient(this.settings.ConnectionString, this.settings.QueueName);
        }
    }
}
