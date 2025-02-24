using System;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using DI.SceneContainerBuilders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Utils.SceneManagement
{
    public interface ISceneLoader
    {
        UniTask LoadGamePlay();
        UniTask LoadMainMenu();
    }

    public class SceneLoader : ISceneLoader, IDisposable
    {
        private SceneInstance? _currentSceneInstance;

        public SceneLoader()
        {
        }

        public async UniTask LoadGamePlay()
        {
            await UnloadCurrentSceneAsync();
            await AsyncLoadBoot();
            await UnloadCurrentSceneAsync();
            await AsyncLoadAndStartGameplay();
        }

        public async UniTask LoadMainMenu()
        {
            await UnloadCurrentSceneAsync();
            await AsyncLoadBoot();
            await UnloadCurrentSceneAsync();
            await AsyncLoadAndStartMainMenu();
        }


        private async UniTask UnloadCurrentSceneAsync()
        {
            if (_currentSceneInstance.HasValue)
            {
                await Addressables.UnloadSceneAsync(_currentSceneInstance.Value).Task;
            }
        }

        private async UniTask AsyncLoadBoot()
        {
            var loading = Addressables.LoadSceneAsync(Scenes.Boot, activateOnLoad: false);
            _currentSceneInstance = await loading.Task;
            await _currentSceneInstance.Value.ActivateAsync();
        }

        private async UniTask AsyncLoadAndStartMainMenu()
        {
            var loading = Addressables.LoadSceneAsync(Scenes.MainMenu, activateOnLoad: false);

            _currentSceneInstance = await loading.Task;

            var parameters = new MainMenuSceneParameters();
            ReflexSceneManager.PreInstallScene(_currentSceneInstance.Value.Scene, parameters.Configure);

            await _currentSceneInstance.Value.ActivateAsync();
        }

        private async UniTask AsyncLoadAndStartGameplay()
        {
            var loading = Addressables.LoadSceneAsync(Scenes.Gameplay, activateOnLoad: false);

            _currentSceneInstance = await loading.Task;

            var parameters = new GameplaySceneParameters();
            ReflexSceneManager.PreInstallScene(_currentSceneInstance.Value.Scene, parameters.Configure);

            await _currentSceneInstance.Value.ActivateAsync();
        }


        public void Dispose()
        {
        }
    }
}