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

    private TreapNode root;
	private TreapNode nullNode;
    private readonly Random random = new Random();

    private void RotateWithLeftChild(ref TreapNode k2){
        TreapNode k1 = k2.left;
        k2.left = k1.right;
        k1.right = k2;
        k2 = k1;

        k1.right.priority = k2.priority;
        k2.priority = Math.Max(k1.left.priority, k1.right.priority) + 1;
    }

    private void RotateWithRightChild(ref TreapNode k1){
        TreapNode k2 = k1.right;
        k1.right = k2.left;
        k2.left = k1;
        k1 = k2;

        k2.left.priority = k1.priority;
        k1.priority = Math.Max(k2.right.priority, k2.left.priority) + 1;
    }

    private TreapNode Insert(int x, TreapNode t, TreapNode parent){
        if (t == nullNode){
			TreapNode new_child=new TreapNode(x, nullNode, nullNode, random.Next());
			new_child.instance=draw_node();
			return new_child;
		}

        else if (x<t.element)
        {
            t.left = Insert(x, t.left,t);
            if (t.left.priority < t.priority)
                RotateWithLeftChild(ref t);
        }
        else if (x>t.element)
        {
            t.right = Insert(x, t.right,t);
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
		node_instance.Position = new Vector2(100, 100);

		return node_instance;
    }

    public void Insert(int x){
		if(root==nullNode){
			root=new TreapNode(x, nullNode, nullNode, random.Next());
			root.instance=draw_node();
			return;
		}
        root = Insert(x, root, null);
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
	private int i = 0;
    private float timer = 0.0f;
    private const float insertDelay = 5.0f;

	public override void _Process(double delta){
		timer += (float)delta;

        // Check if the time delay has elapsed (10 seconds)
        if (timer >= insertDelay)
        {
            // Insert the number i into the Treap
            Insert(i);

            // Output information (optional)
            //GD.Print("Inserted ", i, " into the Treap.");
			DisplayTreeStructure();
			GD.Print("\n");

            // Increment i for the next insertion
            i++;

            // Reset timer for the next insert
            timer = 0.0f;
        }
	}
}