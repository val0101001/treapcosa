using Godot;
using System;

public partial class Treap:Godot.Node{
    private class TreapNode{
        public int element;
		public GDTreapNode instance;
        public TreapNode left;
        public TreapNode right;
        public int height=1;
        public int weight=1;
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

    const float separation_x=125;
    const float separation_y=125;

    private int height(TreapNode n){
        return (n!=null&&n!=nullNode)?n.height:0;
    }

    private int weight(TreapNode n){
        return (n!=null&&n!=nullNode)?n.weight:0;
    }

    private void update_height(ref TreapNode n){
        n.height=Math.Max(height(n.left),height(n.right))+1;
    }

    private void update_weight(ref TreapNode n){
        n.weight=weight(n.left)+weight(n.right)+1;
    }

    private void move_right(ref TreapNode n){
        int w_left=weight(n.left)+1;
        n.instance.Position=new Vector2(
            separation_x*w_left,
            separation_y
        );
    }

    private void move_left(ref TreapNode n){
        int w_right=weight(n.right)+1;
        n.instance.Position=new Vector2(
            -separation_x*w_right,
            separation_y
        );
    }

    private void update_distances(ref TreapNode n){
        if(n.left.instance!=null){
            move_left(ref n.left);
        }
        if(n.right.instance!=null){
            move_right(ref n.right);
        }
    }

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

    private TreapNode Insert(int x, TreapNode t, TreapNode parent, bool left){
        if (t == nullNode){
			TreapNode new_child=new TreapNode(x, nullNode, nullNode, random.Next());
            draw_node(ref new_child,ref parent);

			update_height(ref new_child);
            update_weight(ref new_child);
            update_distances(ref new_child);

            Label label=new_child.instance.GetNode<Label>("Label");
            label.Text=$"{x}";

			return new_child;
		}

        else if (x<t.element)
        {
            t.left = Insert(x, t.left,t,true);
            //if (t.left.priority < t.priority)
              //  RotateWithLeftChild(ref t);
        }
        else if (x>t.element)
        {
            t.right = Insert(x, t.right,t,false);
            //if (t.right.priority < t.priority)
              //  RotateWithRightChild(ref t);
        }

        update_height(ref t);
        update_weight(ref t);
        update_distances(ref t);

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

	private void draw_node(ref TreapNode current,ref TreapNode parent){
        if (TNode == null) return;

		current.instance=(GDTreapNode)TNode.Instantiate();
		if(parent!=nullNode) parent.instance.AddChild(current.instance);
    }

    public void Insert(int x){
		if(root==nullNode){
			root=new TreapNode(x, nullNode, nullNode, random.Next());
            draw_node(ref root,ref nullNode);
			root.instance.Position = new Vector2(500, 100);
            Label label=root.instance.GetNode<Label>("Label");
            label.Text=$"{x}";
            AddChild(root.instance);
			return;
		}
        root = Insert(x, root, null,false);
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
        Insert(500);
	}

	private PackedScene TNode;
	private LineDrawer drawer;
	private int i = 0;

    private void delete(ref TreapNode n){
        n.instance.QueueFree();
        root=nullNode;
        i=0;
        Insert(500);
    }

    // sea el caso de que alguien este leyendo esto, hola xd
    // para debugear permiti controlar la camara con las flechitas
    // insercion de un numero aleatorio del 0 al 999 con enter
    // y borras el arbol entero con esc

	public override void _Process(double delta){
        if(Input.IsActionJustPressed("ui_accept")){
            int x=random.Next()%1000;
            Insert(x);
        }
        if(Input.IsActionJustPressed("ui_cancel")){
            delete(ref root);
        }
	}
}