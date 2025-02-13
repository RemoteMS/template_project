using System;
using Reflex.Core;
using Services.Global;
using Storage;
using UniRx;
using UnityEngine;

namespace DI
{
    public interface ILocalEvent
    {
        public ReactiveProperty<int> Val { get; }
    }

    public class LocalEvent : ILocalEvent, IDisposable
    {
        public ReactiveProperty<int> Val { get; } = new ReactiveProperty<int>(-1);

        public void Dispose()
        {
            Val?.Dispose();
        }
    }

    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            var gameStorage = new GameState();
            builder.AddSingleton(gameStorage, typeof(IGameState), typeof(IDisposable));


            var localEvent = new LocalEvent();
            builder.AddSingleton(localEvent, typeof(ILocalEvent), typeof(LocalEvent));

            builder.AddSingleton<IAudioService>(_ => { return new AudioService(); });

            builder.AddSingleton(typeof(InputManager), new[]
            {
                typeof(IInputManager), typeof(IDisposable),
            });

            builder.AddSingleton(typeof(EventListenerService), new Type[]
            {
                typeof(EventListenerService),
                typeof(ITestListenerService),
                typeof(IEventListenerService),
                typeof(IDisposable),
            });
        }
    }
}