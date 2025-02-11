using _Project.Src.Utils;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App
{
    private static App _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutostartGame()
    {
        // Application.targetFrameRate = 60;
        // Screen.sleepTimeout = SleepTimeout.NeverSleep;

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

        await SceneLoader.LoadSceneAsync(Scenes.Boot);

        var progressSubject = new Subject<float>();
        await SceneLoader.LoadSceneAsync(Scenes.Gameplay, progressSubject);

        // waiting for 
        // await UniTask.Yield();

        Debug.Log($"Loading Gameplay scene complete!");
        progressSubject.Dispose();
    }

    private async UniTask AsyncLoadAndStartMainMenu()
    {
        // _uiRoot.ShowLoadingScreen();
        // _cachedSceneContainer?.Dispose();
        await SceneLoader.LoadSceneAsync(Scenes.Boot);

        var progressSubject = new Subject<float>();
        await SceneLoader.LoadSceneAsync(Scenes.MainMenu, progressSubject);

        // waiting for 
        // await UniTask.Yield();

        Debug.Log($"Loading MainMenu scene complete!");
        progressSubject.Dispose();
    }
}