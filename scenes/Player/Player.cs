using System;
using Godot;

public partial class Player : Area2D
{
    [Export] // This will make the variable editable in the editor
    public int Speed { get; set; } = 400;

    [Signal] // This will make the signal visible in the editor
    public delegate void HitEventHandler();

    public Vector2 ScreenSize;
    private AnimatedSprite2D animatedSprite2D;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        Hide();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 velocity = handleMovementInput(); // The player's movement vector
        animateMovement(velocity, delta);
        updatePosition(velocity, delta);
    }

    private void animateMovement(Vector2 velocity, double delta)
    {
        if (velocity.Length() > 0)
        {
            animatedSprite2D.Play();
            updatePosition(velocity, delta);

            if (velocity.X != 0)
            {
                animatedSprite2D.Animation = "walk";
                animatedSprite2D.FlipV = false;
                animatedSprite2D.FlipH = velocity.X < 0;
            }
            else if (velocity.Y != 0)
            {
                animatedSprite2D.Animation = "up";
                animatedSprite2D.FlipV = velocity.Y > 0;
            }
        }
        else
        {
            animatedSprite2D.Stop();
        }
    }

    private void updatePosition(Vector2 velocity, double delta)
    {
        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
            y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
        );
    }

    private void OnBodyEntered(Node2D body)
    {
        Hide(); // Player disappears after being hit.
        EmitSignal(SignalName.Hit);
        // Must be deferred as we can't change physics properties on a physics callback.
        GetNode<CollisionShape2D>("CollisionShape2D")
            .SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    private Vector2 handleMovementInput()
    {
        var velocity = Vector2.Zero;
        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1;
        }
        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1;
        }
        velocity = velocity.Normalized() * Speed;
        return velocity;
    }

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
}
