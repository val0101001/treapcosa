using Godot;
using System;

public partial class Treap:Godot.Node{
	private class TreapNode{
		public int element;
		public GDTreapNode instance;
		public TreapNode left;
		public TreapNode right;
		public int priority;

		public TreapNode() : this(default(int), null, null, int.MaxValue) { }
		public TreapNode(int e, TreapNode lt, TreapNode rt, int pr){
			element = e;
			left = lt;
			right = rt;
			priority = pr;
		}
	}
	private int height=0;
	private TreapNode root;
	private TreapNode nullNode;
	private readonly Random random = new Random();
	// Para graficar rot_derecha
	private TreapNode hijos_d(ref TreapNode hijo, int val){
		if(hijo==nullNode)
			return nullNode;
		hijo.instance.Position= new Vector2(
			hijo.instance.Position.X-val,
			hijo.instance.Position.Y+(val)
			);
		GD.Print(hijo.left.element.ToString());
		hijo.left=hijos_d(ref hijo.left,val);
		hijo.right=hijos_d(ref hijo.right,val);	
		return hijo;
	}
	private TreapNode hijos_truquito_d(ref TreapNode hijo,int val){
		hijo.instance.Position= new Vector2(
			hijo.instance.Position.X-val,
			hijo.instance.Position.Y+val
			);
		hijo.left=hijos_d(ref hijo.left,val);
		return hijo;
	}
	//grafico rot_izquierdo
	private TreapNode hijos_i(ref TreapNode hijo, int val){
		if(hijo==nullNode)
			return nullNode;
		hijo.instance.Position= new Vector2(
			hijo.instance.Position.X+val,
			hijo.instance.Position.Y+(val)
			);
		GD.Print(hijo.left.element.ToString());
		hijo.left=hijos_i(ref hijo.left,val);
		hijo.right=hijos_i(ref hijo.right,val);	
		return hijo;
	}
	private TreapNode hijos_truquito_i(ref TreapNode hijo,int val){
		hijo.instance.Position= new Vector2(
			hijo.instance.Position.X+val,
			hijo.instance.Position.Y+val
			);
		hijo.right=hijos_i(ref hijo.right,val);
		return hijo;
	}
	private void RotateWithLeftChild(ref TreapNode k2){
		GD.Print("(LEFT)");
		k2=hijos_truquito_i(ref k2,150);
		TreapNode k1 = k2.left;
		k2.left = k1.right;
		k1.right = k2;
		if(k2==root)
			root= k1;
		k2 = k1;
		
		k2.instance.Position= new Vector2(
			k2.instance.Position.X+150,
			k2.instance.Position.Y-150
		);
	}

	private void RotateWithRightChild(ref TreapNode k1){
		GD.Print("(Right)");
		k1=hijos_truquito_d(ref k1,150);
		TreapNode k2 = k1.right;
		k1.right = k2.left;
		k2.left = k1;	
		if(k1==root)
			root= k2;
		k1=k2;
		k1.instance.Position= new Vector2(
			k1.instance.Position.X-150,
			k1.instance.Position.Y-150
		);

	}

private TreapNode Insert(int x, TreapNode t, TreapNode parent, bool left, int level)
{
	if (t == nullNode)
	{
		TreapNode new_child = new TreapNode(x, nullNode, nullNode, random.Next());
		new_child.instance = draw_node();
		new_child.instance.SetLabelText(x.ToString());
		GD.Print($"{height} , {level}");
		// sacale el (1+(0.2*(height-level + 1))) y queda con colision
		double xOffset = (left ? -1 : 1) * 150 *(1+(0.2*(height-level + 1)));
		new_child.instance.Position = new Vector2(
			parent.instance.Position.X + (float)xOffset,
			parent.instance.Position.Y + 150
		);

		GD.Print($"valorInsert: {new_child.element}, priority:{new_child.priority}, EjeX= {new_child.instance.Position.X}, EjeY= {new_child.instance.Position.Y}");
		GD.Print($"valorParent: {parent.element}, priority:{parent.priority}, EjeXParent= {parent.instance.Position.X}, EjeYParent= {parent.instance.Position.Y}");
		// lo agregue para la colision
		height+=1;
		return new_child;
	}
	else if (x < t.element)
	{
		t.left = Insert(x, t.left, t, true, level + 1);
		if (t.left.priority < t.priority)
			RotateWithLeftChild(ref t);
	}
	else if (x > t.element)
	{
		t.right = Insert(x, t.right, t, false, level + 1);
		if (t.right.priority < t.priority)
			RotateWithRightChild(ref t);
	}
	return t;
}


	private TreapNode Remove(int x, TreapNode t){
		if (t != nullNode)
		{
			if (x<t.element)
				t.left = Remove(x, t.left);
			else if (x>t.element)
				t.right = Remove(x, t.right);
			else
			{
				if (t.left == nullNode)
					return t.right;
				else if (t.right == nullNode)
					return t.left;

				if (t.left.priority < t.right.priority)
				{
					RotateWithLeftChild(ref t);
					t.right = Remove(x, t.right);
				}
				else
				{
					RotateWithRightChild(ref t);
					t.left = Remove(x, t.left);
				}
			}
		}
		return t;
	}

	private bool Contains(int x, TreapNode t){
		if (t == nullNode)
			return false;
		else if (x<t.element)
			return Contains(x, t.left);
		else if (x>t.element)
			return Contains(x, t.right);
		else
			return true;
	}

	private void DisplayTreeStructure(TreapNode t, int depth = 0)
	{

		if (t != nullNode)
		{
			DisplayTreeStructure(t.right, depth + 1);
			string tabs = "";
			for (int i = 0; i < depth; ++i)
			{
				tabs += "\t";
			}
			GD.Print($"{tabs}{t.element} (Priority: {t.priority})");
			DisplayTreeStructure(t.left, depth + 1);
		}
	}

	private GDTreapNode draw_node(){
		if (TNode == null) return null;

		var node_instance=(GDTreapNode)TNode.Instantiate();
		AddChild(node_instance);

		return node_instance;
	}

	public void Insert(int x){
		if(root==nullNode){
			root=new TreapNode(random.Next()%100, nullNode, nullNode, random.Next());
			root.instance=draw_node();
			root.instance.SetLabelText(x.ToString());
			root.instance.Position = new Vector2(500, -400);
			GD.Print($"valor: {root.element}, EjeX= {root.instance.Position.X}, EjeY= {root.instance.Position.Y}");
			return;
		}
		root = Insert(random.Next()%100, root, null,false,0);
	}

	public void Remove(int x){
		root = Remove(x, root);
	}

	public bool Contains(int x){
		return Contains(x, root);
	}

	public void DisplayTreeStructure(){
		DisplayTreeStructure(root);
	}

	public override void _Ready(){
		nullNode=new TreapNode();
		nullNode.left=nullNode;
		nullNode.right=nullNode;
		nullNode.priority=int.MaxValue;
		root=nullNode;
		TNode=GD.Load<PackedScene>("res://GDTreapNode.tscn");
	}

	private PackedScene TNode;
	private LineDrawer drawer;
	private int i = 50;
	private float timer = 4.5f;
	private const float insertDelay = 5.0f;

	public override void _Process(double delta){
		timer += (float)delta;

		// Check if the time delay has elapsed (10 seconds)
		if (timer >= insertDelay)
		{
			// Insert the number i into the Treap
			GD.Print("testeo1");
			Insert(i);
			GD.Print("testeo");

			// Output information (optional)
			//GD.Print("Inserted ", i, " into the Treap.");
			DisplayTreeStructure();
			GD.Print("\n");

			// Increment i for the next insertion
			i--;

			// Reset timer for the next insert
			timer = 0.0f;
		}
	}
}
