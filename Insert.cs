using Godot;
using System;

public partial class Insert : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		Pressed+=test;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	void test(){
		GD.Print("test");
	}
	
	public override void _Process(double delta){
		
	}
}
