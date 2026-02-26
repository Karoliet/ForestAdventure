using System.Threading.Tasks;
using Godot;

public partial class Player : CharacterBody2D
{
	[Export]
	private float PLAYER_SPEED = 100.0f;

	private AnimatedSprite2D _animPlayer;

	private bool _isGameOver = false;

	private const string BulletScenePath = "res://Scenes/Bullet.tscn";
	private PackedScene _bulletScene;

	private AudioStreamPlayer _runningAudio;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isGameOver = false;
		_animPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_bulletScene = ResourceLoader.Load<PackedScene>(BulletScenePath);
		_runningAudio = GetNode<AudioStreamPlayer>("Running");

	}

	public override void _Process(double delta)
	{
		if (Velocity == Vector2.Zero | _isGameOver) _runningAudio.Stop();
		else if (!_runningAudio.Playing) _runningAudio.Play();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isGameOver) return;

		Vector2 playerDir = Input.GetVector("LEFT", "RIGHT", "UP", "DOWN");
		Velocity = playerDir * PLAYER_SPEED;

		if (playerDir == Vector2.Zero) _animPlayer.Play("Idle");
		else _animPlayer.Play("Run");

		MoveAndSlide();


	}

	public async Task GameOver()
	{
		if (_isGameOver) return;

		GD.Print(_isGameOver);
		_isGameOver = true;
		_animPlayer.Play("Death");
		GameManager.Instance.ShowGameOver();
		GetNode<AudioStreamPlayer>("DeathSound").Play();

		//死亡不会闪屏
		GetNode<Godot.Timer>("RestartTimer").Start();

		//死亡不会闪屏
		// await ToSignal(GetTree().CreateTimer(3), Godot.Timer.SignalName.Timeout);
		// GetTree().ReloadCurrentScene();

	}

	public void _OnFire()
	{
		if (Velocity != Vector2.Zero | _isGameOver) return;
		var bullentNode = _bulletScene.Instantiate<Node2D>();
		bullentNode.Position = Position + new Vector2(20, 6);

		GetTree().CurrentScene.AddChild(bullentNode);
		GetNode<AudioStreamPlayer>("FireSound").Play();

	}

	private void _ReloadScene()
	{
		
	}

}
