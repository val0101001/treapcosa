using Godot;
using System;

public partial class GDTreapNode : Node2D
{
	// Called when the node enters the scene tree for the first time.
	Label label;
	Sprite2D sprite;
	Sprite2D outline;
	public override void _Ready(){
		label=GetNode<Label>("Value");
		sprite=GetNode<Sprite2D>("Node");
		outline=sprite.GetNode<Sprite2D>("Outline");
		outline.Visible=false;
	}

	public void print(){
		GD.Print(label.Text);
	}

	public void set_outline(bool x){
		//outline.Visible=x;
		outline.Visible=false;
	}

	public string text(){
		return label.Text;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
