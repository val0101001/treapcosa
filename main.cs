using Godot;
using System;

public partial class main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	Button insert;
	Button remove;
	Button find;

	TextEdit text;

	int value;

	Treap treap;

	public override void _Ready(){
		CanvasLayer UI=GetNode<CanvasLayer>("UI");
		Control control=UI.GetNode<Control>("Control");
		ColorRect rect=control.GetNode<ColorRect>("ColorRect");
		insert=rect.GetNode<Button>("Insert");
		remove=rect.GetNode<Button>("Remove");
		find=rect.GetNode<Button>("Find");
		text=rect.GetNode<TextEdit>("NumInput");
		treap=GetNode<Treap>("Treap");

		insert.Pressed+=()=>{treap.begin_insert(value);};
		remove.Pressed+=()=>{treap.begin_remove(value);};
		find.Pressed+=()=>{treap.begin_find(value);};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
		if(text.Text.Trim()!=""){
			value=int.Parse(text.Text);
		}
	}
}
