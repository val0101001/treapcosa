using Godot;
using System;
using System.Collections.Generic;

public partial class LineDrawer : Control // or CanvasItem
{
	// Dictionary to store node connections
	private Dictionary<Node2D, List<Node2D>> nodeConnections = new Dictionary<Node2D, List<Node2D>>();

	// Method to add a connection between two nodes
	public void AddConnection(Node2D nodeA, Node2D nodeB)
	{
		if (nodeA != null && nodeB != null){
			if (!nodeConnections.ContainsKey(nodeA))
			{
				nodeConnections[nodeA] = new List<Node2D>();
			}
			if (!nodeConnections.ContainsKey(nodeB))
			{
				nodeConnections[nodeB] = new List<Node2D>();
			}

			// Add connections both ways (bi-directional)
			nodeConnections[nodeA].Add(nodeB);
			nodeConnections[nodeB].Add(nodeA);

			// Trigger redraw
			//QueueDraw();
		}
		else
		{
			GD.Print("One or both nodes are null.");
		}
	}

	public override void _Draw()
	{

		// Iterate through connections and draw lines
		foreach (var node in nodeConnections.Keys)
		{
			foreach (var connectedNode in nodeConnections[node])
			{
				Vector2 startPos = node.GlobalPosition;
				Vector2 endPos = connectedNode.GlobalPosition;

				// Draw lines between connected nodes
				DrawLine(startPos, endPos, Colors.White);
			}
		}
	}
}
