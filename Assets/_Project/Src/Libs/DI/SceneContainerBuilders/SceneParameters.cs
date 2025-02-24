using Reflex.Core;

namespace DI.SceneContainerBuilders
{
    public abstract class SceneParameters
    {
        public abstract void Configure(ContainerBuilder builder);
    }
}