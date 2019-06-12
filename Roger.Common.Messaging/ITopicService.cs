using System;
using System.Threading.Tasks;

namespace Roger.Common.Messaging
{
    public interface ITopicService<T>
    {
        Task SendAsync(T obj);
    }
}
