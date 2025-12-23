using Godot;
using ServiceSystem;

public partial class GameLevel : Node2D, ITick {
    [Export]
    private SceneId _sceneId;

    private GameClock _gameClock;
    private SceneManager _sceneManager;
    private int _ticksPerStage = 20 * Engine.PhysicsTicksPerSecond;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _gameClock = serviceLocator.GetService<GameClock>(ServiceName.GameClock);
        _sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
        _sceneManager.SetCurrentSceneId(_sceneId);

        _gameClock.AddActiveScene(this, GetInstanceId());
    }

    public override void _ExitTree() {
        _gameClock.RemoveActiveScene(GetInstanceId());
    }

    public void PhysicsTick() { }
}