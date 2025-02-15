using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.SceneManagement;
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
    private readonly SceneLoader _sceneLoader;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutostartGame()
    {
        _instance = new App();
        _instance.RunGame();
    }

    private App()
    {
        _sceneLoader = new SceneLoader();

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
            await _sceneLoader.LoadGamePlay();
            return;
        }

        if (sceneName == Scenes.MainMenu)
        {
            await _sceneLoader.LoadMainMenu();
            return;
        }

        if (sceneName != Scenes.Boot && sceneName != Scenes.TestScene)
        {
            return;
        }
#endif
        await _sceneLoader.LoadGamePlay();
    }
}