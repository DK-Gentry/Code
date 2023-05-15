using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Common.EventBusHelper
{
    /// <summary>
    /// 简单的事件总线
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LocalEventBus<T> where T:class
    {
        public delegate Task LocalEventHander(T t);

        public event LocalEventHander localEventHandler;

        public async Task Publish(T t)
        {
           await localEventHandler(t);
        }
    }
}
