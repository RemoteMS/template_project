using Reflex.Core;
using Services.Global;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Utils.GameObjectInstantiating;
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

    private readonly Container _rootContainer;
    private Container _cachedSceneContainer;
    private readonly ISceneLoader _sceneLoader;
    private IInputManager _input;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutostartGame()
    {
        _instance = new App();
        _instance.RunGame();
    }

    private App()
    {
        _rootContainer = CreateProjectContainer.Create();

        _sceneLoader = _rootContainer.Resolve<ISceneLoader>();

        var asyncSceneLoader = new GameObject("[AsyncSceneLoader]");
        Object.DontDestroyOnLoad(asyncSceneLoader);

        var scopes = new GameObject("[SCOPE]");
        Object.DontDestroyOnLoad(scopes);

        _input = _rootContainer.Resolve<IInputManager>();
    }

    private async void RunGame()
    {
        var dataLoader = _rootContainer.Resolve<IDataLoader>();
        await dataLoader.InitializeAsync();
        await dataLoader.LoadMainAudioMixer();

        var audioManager = await GameObjectInstantiator.InstantiateAudioManager();

        Debug.Log("audioManager.GetAwaiter IsCompleted");
        Object.DontDestroyOnLoad(audioManager);

        var audioService = _rootContainer.Resolve<IAudioService>();
        audioService.Bind(audioManager);
        audioService.InjectAudioMixer(dataLoader.audioMixer);

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