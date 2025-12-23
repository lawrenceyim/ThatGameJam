using Godot;
using ServiceSystem;

public partial class AreaTransitionZone : Area2D {
    [Export]
    private SceneId _nextScene;

    private SceneManager _sceneManager;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
        BodyEntered += _HandleCollision;
    }

    private void _HandleCollision(Node2D body) {
        if (true) { // change to player detection later
            CallDeferred(nameof(_ChangeScene));
        }
    }

    private void _ChangeScene() {
        _sceneManager.ChangeScene(_nextScene);
    }
}