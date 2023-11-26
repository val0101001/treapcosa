using Godot;
using System;

public partial class Selector : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	
	double timer=0;
	double time_direction=1;
	double move_duration=0.5;
	double delta;

	Vector2 begin;
	Vector2 end;
	bool moving=false;
	
	private void move(){
		if(timer>move_duration||timer<0){
			begin=end;
			time_direction*=1;
			timer=0;
			moving=false;
		}

		timer+=delta*time_direction;
		double t=timer/move_duration;

		Position=new Vector2(
			(float)Mathf.Lerp(begin.X,end.X,t),
			(float)Mathf.Lerp(begin.Y,end.Y,t)
		);
	}

	public void set_move(Vector2 pos){
		if(moving) return;

		begin=Position;
		end=pos;

		moving=true;
	}

	public override void _Ready(){
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		this.delta=delta;

		if(moving) move();
	}
}
