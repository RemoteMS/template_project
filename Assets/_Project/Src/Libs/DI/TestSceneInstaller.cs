using DI.SceneContainerBuilders;
using Reflex.Core;
using UnityEngine;

namespace DI
{
    public class TestSceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeReference, SubclassSelector] private SceneParameters sceneParameters;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            sceneParameters.Configure(containerBuilder);
        }
    }
}