using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	private const float speed=500f;
	private const float zoom_speed=0.5f;
	
	public override void _Ready(){

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		Vector2 movement=new Vector2();

		if (Input.IsActionPressed("ui_right"))
            movement.X += 1;
        if (Input.IsActionPressed("ui_left"))
            movement.X -= 1;
        if (Input.IsActionPressed("ui_down"))
            movement.Y += 1;
        if (Input.IsActionPressed("ui_up"))
            movement.Y -= 1;

		GlobalPosition+=movement*speed*(float)delta;

		if(Input.IsActionPressed("ui_minus"))
			Zoom-=new Vector2(
				zoom_speed*(float)delta,
				zoom_speed*(float)delta
			);

		if(Input.IsActionPressed("ui_plus"))
		Zoom+=new Vector2(
			zoom_speed*(float)delta,
			zoom_speed*(float)delta
		);
		
	}
}
