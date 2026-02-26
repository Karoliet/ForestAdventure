using Godot;
using System.Threading.Tasks;

public partial class Bullet : Area2D
{
	private float bulletSpeed = 300;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ = Destroy();
	}

	private async Task Destroy()
	{
		await ToSignal(GetTree().CreateTimer(3), Godot.Timer.SignalName.Timeout);
		QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += new Vector2(bulletSpeed, 0) * (float)delta;
	}
}
