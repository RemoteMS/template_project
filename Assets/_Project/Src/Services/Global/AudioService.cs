using System;

namespace Services.Global
{
    public interface IAudioService
    {
        public string Name { get; }
    }

    public class AudioService : IAudioService, IDisposable
    {
        public string Name { get; } = "AudioService_Name_111";


        public AudioService()
        {
        }

        public void Dispose()
        {
        }
    }
}