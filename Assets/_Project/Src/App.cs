using Cysharp.Threading.Tasks;
using Reflex.Core;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class App
{
    private static App _instance;

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
        // _cachedSceneContainer?.Dispose();

        await Addressables.LoadSceneAsync(Scenes.Boot, activateOnLoad: false);

        var progressSubject = new Subject<float>();

        var asyncOperationHandle = Addressables.LoadSceneAsync(Scenes.Gameplay, activateOnLoad: false);

        asyncOperationHandle.Completed += handle =>
        {
            ReflexSceneManager.PreInstallScene(handle.Result.Scene,
                builder => { builder.SetName($"LoadedFrom_{nameof(AsyncLoadAndStartGameplay)}"); });
            handle.Result.ActivateAsync();
        };


        await asyncOperationHandle;

        // waiting for 
        // await UniTask.Yield();

        Debug.Log($"Loading Gameplay scene complete!");
        progressSubject.Dispose();
    }

    private async UniTask AsyncLoadAndStartMainMenu()
    {
        // _uiRoot.ShowLoadingScreen();
        // _cachedSceneContainer?.Dispose();
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