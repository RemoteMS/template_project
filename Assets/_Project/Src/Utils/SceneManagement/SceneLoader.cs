using System;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Controllers.PlayerControls;
using Services.Gameplay.Units;
using Storage;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using Views;

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

            ReflexSceneManager.PreInstallScene(_currentSceneInstance.Value.Scene, builder =>
            {
                Debug.Log($"LoadedFrom_{nameof(AsyncLoadAndStartMainMenu)}");
                builder.SetName($"LoadedFrom_{nameof(AsyncLoadAndStartMainMenu)}");
                builder.AddSingleton(typeof(MainMenuModelView), new[]
                {
                    typeof(MainMenuModelView),
                    typeof(IModelView),
                    typeof(IDisposable),
                    typeof(IMainMenuModelView),
                });
            });

            await _currentSceneInstance.Value.ActivateAsync();
        }

        private async UniTask AsyncLoadAndStartGameplay()
        {
            var loading = Addressables.LoadSceneAsync(Scenes.Gameplay, activateOnLoad: false);

            _currentSceneInstance = await loading.Task;

            ReflexSceneManager.PreInstallScene(_currentSceneInstance.Value.Scene, builder =>
            {
                builder.SetName($"LoadedFrom_{nameof(AsyncLoadAndStartGameplay)}");

                builder.AddSingleton(new GameplayState(), new[] { typeof(IDisposable), typeof(GameplayState) });

                builder.AddSingleton(typeof(GameplayModelView), new[]
                {
                    typeof(GameplayModelView),
                    typeof(IModelView),
                    typeof(IDisposable),
                    typeof(IGameplayModelView)
                });

                builder.AddSingleton(
                    typeof(UnitSelectionManager),
                    new[] { typeof(UnitSelectionManager) }
                );

                builder.AddSingleton(
                    typeof(PlayerController),
                    new[] { typeof(IPlayerController), typeof(PlayerController) }
                );

                builder.AddSingleton(typeof(ExistUintsService), typeof(ExistUintsService));
            });

            await _currentSceneInstance.Value.ActivateAsync();
        }


        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}