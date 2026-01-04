using Godot;
using System;
using ServiceSystem;

public partial class MainMenu : Node2D {
    [Export]
    private Button _playButton;

    private SceneManager _sceneManager;

    public override void _Ready() {
        ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
        _sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);
        _playButton.Pressed += () => _sceneManager.ChangeScene(SceneId.CutScene);
    }
}