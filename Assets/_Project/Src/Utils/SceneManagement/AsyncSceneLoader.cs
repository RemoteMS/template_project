using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using UniRx;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Utils.SceneManagement
{
    public class AsyncSceneLoader
    {
        private readonly Dictionary<string, SceneInstance> _loadedScenes = new();
        private readonly Subject<string> _onSceneLoaded = new();
        private readonly Subject<string> _onSceneUnloaded = new();

        public IObservable<string> onSceneLoaded => _onSceneLoaded;
        public IObservable<string> onSceneUnloaded => _onSceneUnloaded;

        public async UniTask LoadSceneAsync(string sceneKey, Action<ContainerBuilder> actionBuilder,
            LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (_loadedScenes.ContainsKey(sceneKey)) return;

            var addressableLoadScene = Addressables.LoadSceneAsync(sceneKey, mode, true);
            addressableLoadScene.Completed += handle =>
            {
                ReflexSceneManager.PreInstallScene(handle.Result.Scene, actionBuilder);
            };

            var sceneInstance = await addressableLoadScene;
            _loadedScenes[sceneKey] = sceneInstance;
            _onSceneLoaded.OnNext(sceneKey);
        }

        public async UniTask UnloadSceneAsync(string sceneKey)
        {
            if (!_loadedScenes.TryGetValue(sceneKey, out var sceneInstance)) return;

            await Addressables.UnloadSceneAsync(sceneInstance);
            _loadedScenes.Remove(sceneKey);
            _onSceneUnloaded.OnNext(sceneKey);
        }

        // public async UniTask ReloadSceneAsync(string sceneKey)
        // {
        //     if (!_loadedScenes.ContainsKey(sceneKey)) return;
        //     await UnloadSceneAsync(sceneKey);
        //     await LoadSceneAsync(sceneKey);
        // }
    }
}