using System;

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
        public void Dispose()
        {
        }
    }
}