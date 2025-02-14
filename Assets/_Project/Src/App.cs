using System;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
            await AsyncLoadAndStartGameplay();
            return;
        }

        if (sceneName == Scenes.MainMenu)
        {
            await AsyncLoadAndStartMainMenu();
            return;
        }

        if (sceneName != Scenes.Boot && sceneName != Scenes.TestScene)
        {
            return;
        }
#endif
        await AsyncLoadAndStartGameplay();
    }

    private async UniTask AsyncLoadAndStartGameplay()
    {
        _cachedSceneContainer?.Dispose();

        await Addressables.LoadSceneAsync(Scenes.Boot, activateOnLoad: false);

        var asyncOperationHandle = Addressables.LoadSceneAsync(Scenes.Gameplay, activateOnLoad: false);

        asyncOperationHandle.Completed += handle =>
        {
            ReflexSceneManager.PreInstallScene(handle.Result.Scene, builder =>
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

            handle.Result.ActivateAsync();
        };

        await asyncOperationHandle;
        Debug.Log($"Loading Gameplay scene complete!");
    }

    private async UniTask AsyncLoadAndStartMainMenu()
    {
        _cachedSceneContainer?.Dispose();

        await Addressables.LoadSceneAsync(Scenes.Boot, activateOnLoad: false);

        var asyncOperationHandle = Addressables.LoadSceneAsync(Scenes.MainMenu, activateOnLoad: false);

        asyncOperationHandle.Completed += handle =>
        {
            ReflexSceneManager.PreInstallScene(handle.Result.Scene,
                builder =>
                {
                    Debug.Log($"LoadedFrom_{nameof(AsyncLoadAndStartMainMenu)}");
                    builder.SetName($"LoadedFrom_{nameof(AsyncLoadAndStartMainMenu)}");
                });

            handle.Result.ActivateAsync();
        };

        var a = await asyncOperationHandle;
        Debug.Log($"Loading MainMenu scene complete!");
    }
}