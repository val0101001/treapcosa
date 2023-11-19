using Godot;
using System;

public partial class GDTreapNode : Node2D
{
	private Label label;
	private CollisionShape2D collisionShape;
	private bool hasTextChanged = false;
	public Node2D body;
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		label.Modulate = new Color(0, 0, 0);
		body= GetNode<Node2D>("Area_treap");
	}


	public void SetLabelText(string text)
	{
		label.Text = text;
		hasTextChanged = true;
	}
	public void _on_area_treap_body_entered()
	{
		if (body.IsInGroup("nodo_treap"))
		{
			GD.Print("se winnea");
		}else{
			GD.Print("sexoooooo");
		}
	}
}


