using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utils.GameObjectInstantiating
{
    public static class GameObjectInstantiator
    {
        public static async UniTask<GameObject> InstantiateAudioManager()
        {
            var asyncOperationHandle = Addressables.InstantiateAsync("AudioManager.prefab");


            GameObject gameObject = await asyncOperationHandle.Task;
            return await asyncOperationHandle;
        }
    }
}