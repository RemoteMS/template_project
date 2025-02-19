using System;
using Reflex.Core;
using Services.Global;
using Storage;
using UniRx;
using UnityEngine;
using Utils.GameObjectInstantiating;
using Utils.SceneManagement;

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
            builder.AddSingleton(typeof(DataLoader), new[] { typeof(IDataLoader), typeof(DataLoader) });
            builder.AddSingleton(new SceneLoader(),  new[] { typeof(ISceneLoader), typeof(IDisposable) });

            builder.AddSingleton(typeof(InputManager), new[]
            {
                typeof(IInputManager), typeof(IDisposable),
            });


            var gameStorage = new GameState();
            builder.AddSingleton(gameStorage, typeof(IGameState), typeof(IDisposable));


            var localEvent = new LocalEvent();
            builder.AddSingleton(localEvent, typeof(ILocalEvent), typeof(LocalEvent));

            builder.AddSingleton<IAudioService>(_ => new AudioService());


            builder.AddSingleton(typeof(EventListenerService), new Type[]
            {
                typeof(IEventListenerService),
                typeof(IDisposable),
            });
        }
    }
}