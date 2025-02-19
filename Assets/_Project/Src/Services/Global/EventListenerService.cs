using System;

namespace Services.Global
{
    public interface IEventListenerService
    {
    }

    public class EventListenerService : IEventListenerService, IDisposable
    {
        public void Dispose()
        {
        }
    }
}