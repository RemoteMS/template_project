using System;
using Reflex.Core;
using Services.Global;
using Storage;
using UnityEngine;
using Utils.GameObjectInstantiating;
using Utils.SceneManagement;

namespace DI
{
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

            builder.AddSingleton<IAudioService>(_ => new AudioService());

            builder.AddSingleton(typeof(EventListenerService), new Type[]
            {
                typeof(IEventListenerService),
                typeof(IDisposable),
            });
        }
    }
}