using Godot;
using System;

public partial class GDTreapNode : Node2D
{
	// Called when the node enters the scene tree for the first time.
	Label value;
	Label priority;
	Sprite2D sprite;
	Sprite2D outline;

	double timer=0;
	double time_direction=1;
	double move_duration=0.5;
	double delta;

	Vector2 begin;
	Vector2 end;

	Vector2 global_begin;
	Vector2 global_end;

	bool moving=false;
	bool global_moving=false;

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

	private void global_move(){
		if(timer>move_duration||timer<0){
			global_begin=global_end;
			time_direction*=1;
			timer=0;
			global_moving=false;
		}

		timer+=delta*time_direction;
		double t=timer/move_duration;

		GlobalPosition=new Vector2(
			(float)Mathf.Lerp(global_begin.X,global_end.X,t),
			(float)Mathf.Lerp(global_begin.Y,global_end.Y,t)
		);
	}

	public void set_move(Vector2 pos){
		if(moving||global_moving) return;

		begin=Position;
		end=pos;

		moving=true;
	}

	public void global_set_move(Vector2 pos){
		if(moving||global_moving) return;

		global_begin=GlobalPosition;
		global_end=pos;

		global_moving=true;
	}

	public override void _Ready(){
		value=GetNode<Label>("Value");
		priority=GetNode<Label>("Priority");
		sprite=GetNode<Sprite2D>("Node");
		outline=sprite.GetNode<Sprite2D>("Outline");
		outline.Visible=false;
	}

	public void print(){
		GD.Print(value.Text);
	}

	public void set_visible(bool x){
		outline.Visible=x;
	}

	public string get_value(){
		return value.Text;
	}

	public void set_value(int x){
		value.Text=x.ToString();
	}

	public string get_priority(){
		return priority.Text;
	}

	public void set_priority(int x){
		priority.Text=x.ToString();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		this.delta=delta;

		if(moving) move();
		else if(global_moving) global_move();
	}
}
