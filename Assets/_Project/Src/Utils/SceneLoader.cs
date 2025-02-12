using System;
using Cysharp.Threading.Tasks;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class SceneLoader
    {
        public static async UniTask LoadSceneAsync(string sceneName)
        {
            Debug.Log($"Entered into {sceneName}.");

            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            if (asyncOperation == null)
            {
                throw new ArgumentException($"Scene {sceneName} not found.");
            }

            asyncOperation.allowSceneActivation = false;

            // Track loading progress
            while (asyncOperation.progress < 0.9f)
            {
                await UniTask.Yield(); // Yield control to the next frame
            }

            Debug.Log($"Scene {sceneName} is ready to activate.");

            // Automatically activate the scene when fully loaded
            asyncOperation.allowSceneActivation = true;

            // Wait until the scene is completely activated
            await asyncOperation;
        }


        public static async UniTask LoadSceneAsync(string sceneName, IObserver<float> progressObserver)
        {
            Debug.Log($"Entered into {sceneName}.");

            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            if (asyncOperation == null)
            {
                throw new ArgumentException($"Scene {sceneName} not found.");
            }

            asyncOperation.allowSceneActivation = false;

            // Track loading progress
            while (asyncOperation.progress < 0.9f)
            {
                progressObserver.OnNext(asyncOperation.progress);
                await UniTask.Yield(); // Yield control to the next frame
            }

            progressObserver.OnNext(asyncOperation.progress);
            progressObserver.OnCompleted();

            Debug.Log($"Scene {sceneName} is ready to activate.");

            // Automatically activate the scene when fully loaded
            asyncOperation.allowSceneActivation = true;

            // Wait until the scene is completely activated
            await asyncOperation;
        }
    }
}