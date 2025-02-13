using System;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using Reflex.Extensions;
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
        GlobalContainer.Initialize();
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
                builder =>
                {
                    builder.AddSingleton("Beautiful");
                    builder.AddSingleton(typeof(ScenesTestSSS), new[] { typeof(IDisposable) });
                    builder.AddScoped(
                        container => { return new MyService("test"); }, typeof(IMyService));

                    // IInstaller
                });
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
                builder =>
                {
                    builder.AddSingleton("Beautiful");
                    builder.AddSingleton(new ScenesTest(), typeof(ScenesTest));
                    builder.AddScoped(typeof(ScenesTestSSS), new[] { typeof(IDisposable) });
                    builder.AddTransient(typeof(TestFactory), typeof(ITestfactory), typeof(TestFactory));
                });
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

public class ScenesTest
{
    public string MainScene = "MainScene";
}

public interface ITestfactory
{
}

public class TestFactory : ITestfactory
{
    public string name = "TestFactory";
}

public class ScenesTestSSS : IDisposable
{

    public string MainScene = "ScenesTestSSS";

    public void Dispose()
    {
        Debug.Log("ScenesTestSSS");
    }
}


////////////////////////////////////////////////////////////////////
public interface IMyService
{
    public string Data { get; }
}

public class MyService : IMyService, IDisposable
{
    public string Data { get; } = "MyService_data";

    public MyService()
    {
    }

    public MyService(string data)
    {
        Data = data;
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}

public static class GlobalContainer
{
    private static Container _container;

    public static void Initialize()
    {
        if (_container != null) return;

        var builder = new ContainerBuilder();
        builder.AddSingleton<IMyService>(
            container => { return new MyService("new data"); },
            typeof(IMyService));

        _container = builder.Build();
    }

    public static T Resolve<T>() => _container.Resolve<T>();
}

public class FirstSceneLogic
{
    public void Setup()
    {
        GlobalContainer.Initialize(); // Создаём контейнер
        var service = GlobalContainer.Resolve<MyService>();
        Console.WriteLine($"[First Scene] Получен объект: {service}");
    }
}

public class SecondSceneLogic
{
    public void Load()
    {
        var service = GlobalContainer.Resolve<MyService>();
        Console.WriteLine($"[Second Scene] Получен объект: {service}");
    }
}