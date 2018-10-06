
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Interfaces.Queue
{
    public interface IQueueAccessor<T> where T : class
    {
        Task SendAsync(T item);

        Task Receive(Func<T, Task> onProcess);
    }
}
