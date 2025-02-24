using Reflex.Core;
using UnityEngine;

namespace DI.SceneContainerBuilders
{
    [System.Serializable]
    public class TestSceneParameters : SceneParameters
    {
        public override void Configure(ContainerBuilder builder)
        {
            Debug.LogWarning("Test Scene Parameters Configured;");
        }
    }
}