using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roger.Common.Messaging
{
    public interface ITopicService<T>
        where T: new()
    {
        Task SendAsync(T obj, IDictionary<string, object> properties = null);
    }
}
