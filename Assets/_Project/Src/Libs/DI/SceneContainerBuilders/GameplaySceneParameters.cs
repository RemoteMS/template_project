using System;
using Controllers.PlayerControls;
using Reflex.Core;
using Services.Gameplay.Units;
using Storage;
using Views;

namespace DI.SceneContainerBuilders
{
    public class GameplaySceneParameters : SceneParameters
    {
        public override void Configure(ContainerBuilder builder)
        {
            builder.SetName($"LoadedFrom_AsyncLoadAndStartGameplay");

            builder.AddSingleton(new GameplayState(), new[] { typeof(IDisposable), typeof(GameplayState) });

            builder.AddSingleton(typeof(GameplayModelView), new[]
            {
                typeof(GameplayModelView),
                typeof(IModelView),
                typeof(IDisposable),
                typeof(IGameplayModelView)
            });

            builder.AddSingleton(
                typeof(UnitSelectionManager),
                new[] { typeof(UnitSelectionManager) }
            );

            builder.AddSingleton(
                typeof(PlayerController),
                new[] { typeof(IPlayerController), typeof(PlayerController) }
            );

            builder.AddSingleton(typeof(ExistUintsService), typeof(ExistUintsService));
        }
    }
}