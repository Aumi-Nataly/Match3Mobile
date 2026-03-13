using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class LevelLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<GridSystem>();
        builder.Register<IMatchFinder, MatchFinder>(Lifetime.Singleton);
        builder.Register<IFallTile, FallTile>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<InputService>();
        builder.RegisterComponentInHierarchy<SwipeDetection>();
    }
}
