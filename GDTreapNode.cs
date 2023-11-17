using Godot;
using System;

public partial class GDTreapNode : Node2D
{
	public Label label;
	private bool hasTextChanged = false;

	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		label.Modulate = new Color(0, 0, 0);
	}

	// Método para establecer el texto
	public void SetLabelText(string text)
	{
		label.Text = text;
		hasTextChanged = true;
	}
}
