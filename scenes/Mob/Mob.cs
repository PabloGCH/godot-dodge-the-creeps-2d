using Godot;
using System;

public partial class Mob : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
    var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames(); // Get the list of animations
    animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]); //GD.Randi() % mobTypes.Length will return a random number between 0 and the number of animations
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

  private void OnVisibleOnScreenNotifier2DScreenExited()
  {
      QueueFree(); // Queue the node for deletion
  }
}
