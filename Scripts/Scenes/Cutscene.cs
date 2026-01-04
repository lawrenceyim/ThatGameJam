using Godot;
using System;
using ServiceSystem;

public partial class Cutscene : Node2D {
	[Export]
	private Camera2D _camera2D;

	private TickTimer _timer = new();
	private int _duration = 7 * Engine.PhysicsTicksPerSecond;
	private SceneManager _sceneManager;
	private Vector2 _cameraMovement = new Vector2(1, 0);

	public override void _Ready() {
		ServiceLocator serviceLocator = GetNode<ServiceLocator>(ServiceLocator.AutoloadPath);
		_sceneManager = serviceLocator.GetService<SceneManager>(ServiceName.SceneManager);

		_timer.TimedOut += _ChangeScene;
		_timer.StartFixedTimer(false, _duration);
	}

	public override void _PhysicsProcess(double delta) {
		_camera2D.Position += _cameraMovement;
		_timer.PhysicsTick();
	}

	private void _ChangeScene() {
		_sceneManager.ChangeScene(SceneId.LevelOne);
	}
}
