using System;
using Reflex.Core;
using Views;

namespace DI.SceneContainerBuilders
{
    public class MainMenuSceneParameters : SceneParameters
    {
        public override void Configure(ContainerBuilder builder)
        {
            builder.SetName($"LoadedFrom_{nameof(MainMenuSceneParameters)}");
            builder.AddSingleton(typeof(MainMenuModelView), new[]
            {
                typeof(MainMenuModelView),
                typeof(IModelView),
                typeof(IDisposable),
                typeof(IMainMenuModelView),
            });
        }
    }
}