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
        Object.DontDestroyOnLoad(asyncSceneLoader.gameObject);

        var scopes = new GameObject("[SCOPE]");
        Object.DontDestroyOnLoad(scopes.gameObject);

        // Container rootContainer = ReflexContainer.Root;
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

        if (sceneName != Scenes.Boot || sceneName != Scenes.TestScene)
        {
            return;
        }
#endif
        await AsyncLoadAndStartGameplay();
    }


    private async UniTask AsyncLoadAndStartGameplay()
    {
        // _uiRoot.ShowLoadingScreen();
        _cachedSceneContainer?.Dispose();

        await Addressables.LoadSceneAsync(Scenes.Boot, activateOnLoad: false);

        var progressSubject = new Subject<float>();

        var asyncOperationHandle = Addressables.LoadSceneAsync(Scenes.Gameplay, activateOnLoad: false);

        asyncOperationHandle.Completed += handle =>
        {
            ReflexSceneManager.PreInstallScene(handle.Result.Scene,
                builder =>
                {
                    builder.SetName($"LoadedFrom_{nameof(AsyncLoadAndStartGameplay)}");
                    builder.AddSingleton(
                        typeof(GameplayModelView)
                        , new[]
                        {
                            typeof(GameplayModelView),
                            typeof(IModelView), typeof(IDisposable),
                            typeof(GameplayModelView)
                        }
                    );
                });
            handle.Result.ActivateAsync();
        };


        await asyncOperationHandle;
        // _cachedSceneContainer = 

        // waiting for 
        // await UniTask.WaitForSeconds(10);

        Debug.Log($"Loading Gameplay scene complete!");
        progressSubject.Dispose();
    }

    private async UniTask AsyncLoadAndStartMainMenu()
    {
        // _uiRoot.ShowLoadingScreen();
        _cachedSceneContainer?.Dispose();

        await Addressables.LoadSceneAsync(Scenes.Boot, activateOnLoad: false);

        var progressSubject = new Subject<float>();

        var asyncOperationHandle = Addressables.LoadSceneAsync(Scenes.MainMenu, activateOnLoad: false);

        asyncOperationHandle.Completed += handle =>
        {
            ReflexSceneManager.PreInstallScene(
                handle.Result.Scene,
                builder => { builder.SetName($"LoadedFrom_{nameof(AsyncLoadAndStartMainMenu)}"); });
            handle.Result.ActivateAsync();
        };

        await asyncOperationHandle;

        // waiting for 
        // await UniTask.Yield();

        Debug.Log($"Loading MainMenu scene complete!");
        progressSubject.Dispose();

        var gameObject = new GameObject();
    }
}