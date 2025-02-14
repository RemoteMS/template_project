using System;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Utils.SceneManagement
{
    public class SceneLoader : IDisposable
    {
        private SceneInstance? _currentSceneInstance;


        public void Dispose()
        {
        }
    }
}