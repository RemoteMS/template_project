using Reflex.Attributes;
using Reflex.Extensions;
using Services.Global;
using Storage;
using UnityEngine;

namespace DI
{
    public class Test : MonoBehaviour
    {
        [Inject] private IGameState _gameState;

        private void Start()
        {
            var sceneContainer = gameObject.scene.GetSceneContainer();
            var audioService = sceneContainer.Resolve<IAudioService>();

            Debug.Log($"Resolver of {nameof(audioService)} {audioService?.Name}");

            var inputManager = sceneContainer.Resolve<IInputManager>();

            Debug.Log($"Resolver of {nameof(inputManager)} {inputManager?.Name}");

            var eventListenerService = sceneContainer.Resolve<EventListenerService>();

            Debug.Log($"Resolver of {nameof(eventListenerService)} {eventListenerService._localEvent.Val.Value}");

            var gameState = sceneContainer.Resolve<IGameState>();

            Debug.Log($"Resolver of {nameof(gameState)} {gameState.Contex.Count}");

            Debug.Log($"Resolver of local {nameof(_gameState)} {_gameState?.Contex?.Count}");
        }
    }
}