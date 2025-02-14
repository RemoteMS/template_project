using System;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Views;
using Object = UnityEngine.Object;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//     !!!!!!!!!!!! IMPORTANT !!!!!!!!!!!!
//
//     The project uses Reflex DI. This involves the use of the ProjectScope.cs files AND the ProjectInstaller.cs files
//     Assets/_Project/Src/Libs/DI/ProjectInstaller.cs.
//
//     App should be the entry point via [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//     , but approximately the same logic is used when creating ProjectScope
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class App
{
    private static App _instance;
    private Container _rootContainer;
    private Container _cachedSceneContainer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutostartGame()
    {
        _instance = new App();
        _instance.RunGame();
    }

    private App()
    {
        var asyncSceneLoader = new GameObject("[AsyncSceneLoader]");
        Object.DontDestroyOnLoad(asyncSceneLoader);

        var scopes = new GameObject("[SCOPE]");
        Object.DontDestroyOnLoad(scopes);
    }

    private async void RunGame()
    {
#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == Scenes.Gameplay)
        {
            await UnloadCurrentSceneAsync();
            await AsyncLoadBoot();
            await AsyncLoadAndStartGameplay();

            await UniTask.WaitForSeconds(6);

            await UnloadCurrentSceneAsync();
            await AsyncLoadBoot();
            await AsyncLoadAndStartMainMenu();

            return;
        }

        if (sceneName == Scenes.MainMenu)
        {
            await AsyncLoadBoot();
            await AsyncLoadAndStartMainMenu();
            return;
        }

        if (sceneName != Scenes.Boot && sceneName != Scenes.TestScene)
        {
            return;
        }
#endif
        await UnloadCurrentSceneAsync();
        await AsyncLoadBoot();
        await AsyncLoadAndStartGameplay();
    }


    private SceneInstance? _currentSceneInstance;

    private async UniTask LoadSceneAsync(string sceneAddress)
    {
        if (_currentSceneInstance.HasValue)
        {
            await Addressables.UnloadSceneAsync(_currentSceneInstance.Value).Task;
        }

        var loading = Addressables.LoadSceneAsync(sceneAddress);
        _currentSceneInstance = await loading.Task;
    }

    private async UniTask UnloadCurrentSceneAsync()
    {
        if (_currentSceneInstance.HasValue)
        {
            await Addressables.UnloadSceneAsync(_currentSceneInstance.Value).Task;
        }
    }

    private async UniTask LoadSceneAsyncWithOutOverride(string sceneAddress)
    {
        var loading = Addressables.LoadSceneAsync(sceneAddress);
        await loading.Task;
    }

    private async UniTask LoadSceneAsyncWithOverride(string sceneAddress)
    {
        var loading = Addressables.LoadSceneAsync(sceneAddress);
        _currentSceneInstance = await loading.Task;
    }

    private async UniTask AsyncLoadBoot()
    {
        var loading = Addressables.LoadSceneAsync(Scenes.Boot, activateOnLoad: false);
        loading.Completed += async => { async.Result.ActivateAsync(); };

        await loading.Task;
    }

    private async UniTask AsyncLoadAndStartMainMenu()
    {
        var loading = Addressables.LoadSceneAsync(Scenes.MainMenu, activateOnLoad: false);

        loading.Completed += async =>
        {
            ReflexSceneManager.PreInstallScene(async.Result.Scene,
                builder =>
                {
                    Debug.Log($"LoadedFrom_{nameof(AsyncLoadAndStartMainMenu)}");
                    builder.SetName($"LoadedFrom_{nameof(AsyncLoadAndStartMainMenu)}");
                });

            async.Result.ActivateAsync();
        };
        _currentSceneInstance = await loading.Task;
    }

    private async UniTask AsyncLoadAndStartGameplay()
    {
        var loading = Addressables.LoadSceneAsync(Scenes.Gameplay, activateOnLoad: false);

        loading.Completed += async =>
        {
            ReflexSceneManager.PreInstallScene(async.Result.Scene, builder =>
            {
                builder.SetName($"LoadedFrom_{nameof(AsyncLoadAndStartGameplay)}");

                builder.AddSingleton(typeof(GameplayModelView), new[]
                {
                    typeof(GameplayModelView),
                    typeof(IModelView),
                    typeof(IDisposable),
                    typeof(GameplayModelView)
                });
            });

            async.Result.ActivateAsync();
        };
        _currentSceneInstance = await loading.Task;
    }
}