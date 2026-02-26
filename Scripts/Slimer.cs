using Godot;
using System.Threading.Tasks;

public partial class Slimer : Area2D
{

	private AnimatedSprite2D _animSlimer;
	private bool _isDeath = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animSlimer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!_isDeath) Position += new Vector2(-50, .0f) * (float)delta;

		if (Position.X < -267) QueueFree();
	}

	private void _on_body_entered(Node2D body)
	{

		if ((body is Player player) && !_isDeath) _ = player.GameOver();
	}

	private void _onAreaEntered(Area2D area)
	{
		if (!area.IsInGroup("bullet")) return;

		_isDeath = true;
		area.QueueFree();
		_ = Death();
		GetNode<AudioStreamPlayer>("DeathSound").Play();

	}

	private async Task Death()
	{
		_animSlimer.Play("Death");
		await ToSignal(GetTree().CreateTimer(0.6), Godot.Timer.SignalName.Timeout);
		QueueFree();
		GameManager.Instance.score += 1;
	}

}
