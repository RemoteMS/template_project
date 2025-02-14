using System;
using DI;

namespace Services.Global
{
    public interface IEventListenerService
    {
    }

    public interface ITestListenerService
    {
    }


    public class EventListenerService : IEventListenerService, ITestListenerService, IDisposable
    {
        public LocalEvent _localEvent;

        public EventListenerService(LocalEvent localEvent)
        {
            _localEvent = localEvent;
        }


        public void Dispose()
        {
        }
    }
}