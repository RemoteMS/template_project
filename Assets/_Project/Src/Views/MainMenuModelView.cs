using System;
using Reflex.Attributes;
using UniRx;
using UnityEngine;
using Utils.SceneManagement;

namespace Views
{
    public interface IMainMenuModelView
    {
    }

    public class MainMenuModelView : IMainMenuModelView, IModelView, IDisposable
    {
        private readonly ISceneLoader _sceneLoader;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        [ReflexConstructor]
        public MainMenuModelView(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void GoToGamePlay()
        {
            _sceneLoader.LoadGamePlay();
            Debug.Log("Go to GoToGamePlay");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}