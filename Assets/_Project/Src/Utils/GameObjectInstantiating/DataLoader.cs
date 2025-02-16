using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Audio;

namespace Utils.GameObjectInstantiating
{
    public interface IDataLoader
    {
        AudioMixer audioMixer { get; }

        UniTask LoadMainAudioMixer();
        UniTask InitializeAsync();
    }

    public class DataLoader : IDataLoader, IDisposable
    {
        private IResourceLocator _locator;
        public AudioMixer audioMixer { get; private set; }

        public DataLoader()
        {
        }

        public void Dispose()
        {
        }

        public async UniTask LoadMainAudioMixer()
        {
            var handle = Addressables.LoadAssetAsync<AudioMixer>(AddressablesConstants.MainGameMixer);

            audioMixer = await handle.Task;


            if (audioMixer == null)
            {
                UnityEngine.Debug.LogError("Не удалось загрузить AudioMixer.");
                return;
            }

            UnityEngine.Debug.Log("AudioMixer успешно загружен.");
        }

        public async UniTask InitializeAsync()
        {
            _locator = await Addressables.InitializeAsync().Task;
        }
    }
}