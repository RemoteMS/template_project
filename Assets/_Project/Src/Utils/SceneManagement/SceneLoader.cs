using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Utils.SceneManagement
{
    public class SceneLoader : IDisposable
    {
        private readonly string _key;
        private AsyncOperationHandle<SceneInstance> _loadHandle;

        private readonly CompositeDisposable _disposables = new();

        public SceneLoader(string key)
        {
            _key = key;
        }

        public void LoadScene()
        {
            _loadHandle = Addressables.LoadSceneAsync(_key, LoadSceneMode.Additive);
        }

        public async UniTask<AsyncOperationHandle<SceneInstance>> UnloadSceneAsync()
        {
            return Addressables.UnloadSceneAsync(_loadHandle);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            UnloadSceneAsync();
        }
    }
}