using System;
using Reflex.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class GameplayUIView : MonoBehaviour, IDisposable
    {
        [SerializeField] Button toMainMenuButton;

        private readonly CompositeDisposable _disposables = new();

        // [Inject] does't work
        // [Inject]
        public GameplayModelView _gameplayModelView;

        private void Start()
        {
            var sceneContainer = gameObject.scene.GetSceneContainer();
            
            _gameplayModelView = sceneContainer.Resolve<GameplayModelView>();


            toMainMenuButton.OnClickAsObservable().Subscribe(
                _ =>
                {
                    // _localEvent.Val.Value++;
                    Debug.Log($"Gameplay UI View clicked ");
                }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}