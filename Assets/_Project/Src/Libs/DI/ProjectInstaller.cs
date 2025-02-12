using Reflex.Core;
using UnityEngine;

namespace DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton("Hello");
        }
    }
}