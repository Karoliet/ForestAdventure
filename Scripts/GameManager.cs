using Godot;
using System;

public partial class GameManager : Node2D
{
	private const string _slimerScenePath = "res://Scenes/Slimer.tscn";
	private PackedScene _slimeScene;

	private Godot.Timer _spawnTimer;

	[Export]
	public int score = 0;

	private Label label;
	private Label gameOverLabel;

	public static GameManager Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_slimeScene = ResourceLoader.Load<PackedScene>(_slimerScenePath);
		_spawnTimer = GetNode<Godot.Timer>("Timer");
		label = GetNode<Label>("CanvasLayer/ScoreLabel");
		gameOverLabel = GetNode<Label>("CanvasLayer/GameOverLabel");

		// 初始化单例（确保全局唯一）
		if (Instance != null && Instance != this) return;
		Instance = this;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_spawnTimer.WaitTime -= 0.2 * delta;
		_spawnTimer.WaitTime = Godot.Mathf.Clamp(_spawnTimer.WaitTime, 1, 3);

		label.Text = "Score:" + score;
	}

	private void _SpawnSlimer()
	{
		var slimeNode = _slimeScene.Instantiate<Area2D>();
		int randomPos = new Random().Next(50, 113);
		slimeNode.Position = new Vector2(374, randomPos);

		GetTree().CurrentScene.AddChild(slimeNode);
	}

	public void ShowGameOver()
	{
		gameOverLabel.Visible = true;
	}
    public override void _ExitTree()
    {
        if (Instance == this)Instance = null;

    }
}
