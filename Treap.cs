using Godot;
using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;

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

    Selector selector;

    const float separation_x=125;
    const float separation_y=150;

    double delta;

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

    private void print_children(TreapNode n){
        GD.Print("parent: ",n.instance.text());
        GD.Print("left: ",(n.left.instance!=null)?n.left.instance.text():"null");
        GD.Print("right: ",(n.right.instance!=null)?n.right.instance.text():"null");
        GD.Print("");
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
        rotate_with_left(ref k2);

        TreapNode k1 = k2.left;
        k2.left = k1.right;
        k1.right = k2;

        if(k1==root){
            root=k1;
        }

        k2 = k1;
        
        //k1.right.priority = k2.priority;
        //k2.priority = Math.Max(k1.left.priority, k1.right.priority) + 1;

        reorder_tree(ref root);
    }

    // this
    private void RotateWithRightChild(ref TreapNode k1){
        rotate_with_right(ref k1);

        TreapNode k2 = k1.right;
        k1.right = k2.left;
        k2.left = k1;

        if(k1==root){
            root=k1;
        }

        k1 = k2;

        //k2.left.priority = k1.priority;
        //k1.priority = Math.Max(k2.right.priority, k2.left.priority) + 1;
        
        reorder_tree(ref root);
    }

    private void rotate_with_right(ref TreapNode k1){
        if(k1!=root){
            GDTreapNode parent=k1.instance.GetParent<GDTreapNode>();
            TreapNode k2=k1.right;
            GDTreapNode k2_left=k2.left.instance;
            if(k2_left!=null) k2.instance.RemoveChild(k2_left);
            if(k2_left!=null) k1.instance.AddChild(k2_left);
            k1.instance.RemoveChild(k2.instance);
            parent.RemoveChild(k1.instance);
            k2.instance.AddChild(k1.instance);
            parent.AddChild(k2.instance);
        }
        else{
            TreapNode k2=k1.right;
            GDTreapNode k2_left=k2.left.instance;
            if(k2_left!=null) k2.instance.RemoveChild(k2_left);
            if(k2_left!=null) k1.instance.AddChild(k2_left);
            k1.instance.RemoveChild(k2.instance);
            RemoveChild(k1.instance);
            k2.instance.AddChild(k1.instance);
            AddChild(k2.instance);   
        }
    }

    private void rotate_with_left(ref TreapNode k1){
        if(k1!=root){
            GDTreapNode parent=k1.instance.GetParent<GDTreapNode>();
            TreapNode k2=k1.left;
            GDTreapNode k2_right=k2.right.instance;
            if(k2_right!=null) k2.instance.RemoveChild(k2_right);
            if(k2_right!=null) k1.instance.AddChild(k2_right);
            k1.instance.RemoveChild(k2.instance);
            parent.RemoveChild(k1.instance);
            k2.instance.AddChild(k1.instance);
            parent.AddChild(k2.instance);
        }
        else{
            TreapNode k2=k1.left;
            GDTreapNode k2_right=k2.right.instance;
            if(k2_right!=null)k2.instance.RemoveChild(k2_right);
            if(k2_right!=null) k1.instance.AddChild(k2_right);
            k1.instance.RemoveChild(k2.instance);
            RemoveChild(k1.instance);
            k2.instance.AddChild(k1.instance);
            AddChild(k2.instance);  
        }
    }

    void reorder_tree(ref TreapNode n){
        update_height(ref n);
        update_weight(ref n);
        update_distances(ref n);
        if(n.left!=null&&n.left!=nullNode) reorder_tree(ref n.left);
        if(n.right!=null&&n.right!=nullNode) reorder_tree(ref n.right);
    }

    const float time_delay=0.5f;

    private async Task<TreapNode> Insert(int x, TreapNode t, TreapNode parent){
        if (t == nullNode){
            parent.instance.set_outline(true);

			TreapNode new_child=new TreapNode(x, nullNode, nullNode, random.Next());
            draw_node(ref new_child,ref parent);
            Label label=new_child.instance.GetNode<Label>("Value");
            label.Text=$"{x}";

            parent.instance.set_outline(false);

			update_height(ref new_child);
            update_weight(ref new_child);
            update_distances(ref new_child);

            new_child.instance.set_outline(true);

            await ToSignal(GetTree().CreateTimer(time_delay),"timeout");

            new_child.instance.set_outline(false);            

			return new_child;
		}

        if(parent!=null&&parent.instance!=null) parent.instance.set_outline(false);
        t.instance.set_outline(true);

        selector.set_move(t.instance.GlobalPosition);
        await ToSignal(GetTree().CreateTimer(time_delay),"timeout");

        if(x<t.element){
            t.left=await Insert(x,t.left,t);
            if(t.left.priority<t.priority){
                RotateWithLeftChild(ref t);
            }
        }
        else if(x>t.element){
            t.right=await Insert(x, t.right,t);
            if(t.right.priority<t.priority){
                RotateWithRightChild(ref t);
            }
        }

        t.instance.set_outline(false);

        update_height(ref t);
        update_weight(ref t);
        update_distances(ref t);

        reorder_tree(ref root);

        traversal.Push(t.instance.GlobalPosition);

        return t;
    }

    Stack<Vector2> traversal=new Stack<Vector2>();

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
                    //RotateWithLeftChild(ref t);
                    t.right = Remove(x, t.right);
                }
                else
                {
                    //RotateWithRightChild(ref t);
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

    public async void Insert(int x){
		if(root==nullNode){
			root=new TreapNode(x, nullNode, nullNode, random.Next());
            draw_node(ref root,ref nullNode);
			root.instance.Position = new Vector2(0,0);
            Label label=root.instance.GetNode<Label>("Value");
            label.Text=$"{x}";
            AddChild(root.instance);
            DisplayTreeStructure();
			return;
		}
        root=await Insert(x, root, null);
        root.instance.Position = new Vector2(0,0);
        DisplayTreeStructure();
        selector.set_move(new Vector2(0,0));
    }

    public void Remove(int x){
        root = Remove(x, root);
    }

    public bool Contains(int x){
        return Contains(x, root);
    }

    public void DisplayTreeStructure(){
        DisplayTreeStructure(root);
        GD.Print("");
    }

    public void display_traversal(){
        string s="";
        foreach(Vector2 pos in traversal){
            s+="("+pos.X+","+pos.Y+") ";
        }
        GD.Print(s);
        traversal.Clear();
    }

	public override void _Ready(){
		nullNode=new TreapNode();
		nullNode.left=nullNode;
		nullNode.right=nullNode;
		nullNode.priority=int.MaxValue;
		root=nullNode;
		TNode=GD.Load<PackedScene>("res://GDTreapNode.tscn");
        selector=GetNode<Selector>("Selector");
        start();
	}

	private PackedScene TNode;
	private LineDrawer drawer;
	private int i = 0;

    private void delete(ref TreapNode n){
        n.instance.QueueFree();
        start();
    }

    private void start(){
        xd=new List<int>(){242,688,893};
        i=0;
    }

    // sea el caso de que alguien este leyendo esto, hola xd
    // para debugear permiti controlar la camara con las flechitas
    // insercion de un numero aleatorio del 0 al 999 con enter
    // y borras el arbol entero con esc

    List<int> xd;

	public override void _Process(double delta){
        if(Input.IsActionJustPressed("ui_accept")){
            int x=random.Next()%1000;
            Insert(x);
            /*if(xd.Count!=0){
                Insert(xd.First());
                xd.Remove(xd.First());
                return;
            }
            RotateWithRightChild(root,null);
            reorder_tree(ref root);*/
        }
        if(Input.IsActionJustPressed("ui_cancel")){
            delete(ref root);
        }
	}
}