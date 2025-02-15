using System;
using Reflex.Attributes;
using UniRx;
using UnityEngine;
using Utils.SceneManagement;

namespace Views
{
    public interface IGameplayModelView
    {
    }

    public class GameplayModelView : IGameplayModelView, IModelView, IDisposable
    {
        public IReadOnlyReactiveProperty<int> TestProp => _testProp;
        private readonly ReactiveProperty<int> _testProp;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private readonly ISceneLoader _sceneLoader;

        // add model
        [ReflexConstructor]
        public GameplayModelView(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;

            _testProp = new ReactiveProperty<int>();

            _testProp.AddTo(_disposable);
        }

        public void GoToMainMenu()
        {
            _sceneLoader.LoadMainMenu();
            Debug.Log("Go to main menu");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}