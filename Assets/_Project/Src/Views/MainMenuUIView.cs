using System;
using Reflex.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MainMenuUIView : MonoBehaviour, IDisposable
    {
        [SerializeField] Button toMainMenuButton;

        private readonly CompositeDisposable _disposables = new();


        private MainMenuModelView _mainMenuModelView;

        private void Start()
        {
            var sceneContainer = gameObject.scene.GetSceneContainer();

            _mainMenuModelView = sceneContainer.Resolve<MainMenuModelView>();


            toMainMenuButton.OnClickAsObservable().Subscribe(
                _ =>
                {
                    Debug.Log($"{nameof(MainMenuModelView)}  View clicked");
                    _mainMenuModelView.GoToGamePlay();
                }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}