using Godot;
using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic.FileIO;

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

	private void update_height(TreapNode n){
		n.height=Math.Max(height(n.left),height(n.right))+1;
	}

	private void update_weight(TreapNode n){
		n.weight=weight(n.left)+weight(n.right)+1;
	}

	private async Task move_right(TreapNode n){
		if(n.left!=null&&n.left!=nullNode) update_weight(n.left);
		update_weight(n);

		int w_left=weight(n.left)+1;

		n.instance.set_move(new Vector2(
			separation_x*w_left,
			separation_y
		));

		await ToSignal(GetTree().CreateTimer(time_delay),"timeout");
	}

	private async Task move_left(TreapNode n){
		if(n.right!=null&&n.right!=nullNode) update_weight(n.right);
		update_weight(n);

		int w_right=weight(n.right)+1;
		
		n.instance.set_move(new Vector2(
			-separation_x*w_right,
			separation_y
		));

		await ToSignal(GetTree().CreateTimer(time_delay),"timeout");
	}

	private async Task update_distances(TreapNode n){
		if(n.left.instance!=null){
			await move_left(n.left);
		}
		if(n.right.instance!=null){
			await move_right(n.right);
		}
	}

	private TreapNode RotateWithLeftChild(TreapNode k2,TreapNode parent){
		rotate_with_left(ref k2);

		TreapNode k1 = k2.left;
		k2.left = k1.right;
		k1.right = k2;

		if(k2==root||parent==null){
			root=k1;
			root.instance.Position=new Vector2(0,0);
			if(k2.element<root.element) root.left=k2;
			else root.right=k2;
		}
		else if(parent.left==k2){
			parent.left=k1;
			GD.Print("parent left: ",parent.element);
		}
		else{
			parent.right=k1;
			GD.Print("parent right: ",parent.element);
		}

		reorder_tree(root);

		return k1;
	}

	private TreapNode RotateWithRightChild(TreapNode k1,TreapNode parent){
		rotate_with_right(ref k1);

		TreapNode k2 = k1.right;
		k1.right = k2.left;
		k2.left = k1;

		if(k1==root||parent==null){
			root=k2;
			root.instance.Position=new Vector2(0,0);
			if(k1.element<root.element) root.left=k1;
			else root.right=k1;
		}
		else if(parent.left==k1){
			parent.left=k2;
			GD.Print("parent left: ",parent.element);
		}
		else{
			parent.right=k2;
			GD.Print("parent right: ",parent.element);
		}
		
		reorder_tree(root);

		return k2;
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

	private async Task reorder_tree(TreapNode n){
		if(n.left!=null&&n.left!=nullNode) await reorder_tree(n.left);
		if(n.right!=null&&n.right!=nullNode) await reorder_tree(n.right);
		update_height(n);
		update_weight(n);
		update_distances(n);
	}

	private void no_anim_reorder(TreapNode n){
		if(n.left!=null&&n.left!=nullNode) no_anim_reorder(n.left);
		if(n.right!=null&&n.right!=nullNode) no_anim_reorder(n.right);

		update_height(n);
		update_weight(n);

		if(n.left.instance!=null){
			int w_right=weight(n.left.right)+1;
			n.left.instance.Position=new Vector2(
				-separation_x*w_right,
				separation_y
			);
		}
		if(n.right.instance!=null){
			int w_left=weight(n.right.left)+1;
			n.right.instance.Position=new Vector2(
				separation_x*w_left,
				separation_y
			);
		}
	}

	const float time_delay=0.5f;

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
		current.instance.Visible=false;
		if(parent!=nullNode) parent.instance.AddChild(current.instance);
	}
	//quizas sea por esto, modificaron todo el treap ya fue todo
	private LineDrawer draw_line_drawer()
	{

		var line_drawer_instance = new LineDrawer(); 
		AddChild(line_drawer_instance);

		return line_drawer_instance;
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

	public override void _Ready(){
		nullNode=new TreapNode();
		nullNode.left=nullNode;
		nullNode.right=nullNode;
		nullNode.priority=int.MaxValue;
		root=nullNode;
		TNode=GD.Load<PackedScene>("res://GDTreapNode.tscn");
		selector=GetNode<Selector>("Selector");
	}

	private PackedScene TNode;
	private LineDrawer drawer;

	// sea el caso de que alguien este leyendo esto, hola xd
	// para debugear permiti controlar la camara con las flechitas
	// insercion de un numero aleatorio del 0 al 999 con enter
	// y borras el arbol entero con esc

	TreapNode current=null;
	TreapNode prev=null;

	void wasd(){
		if(Input.IsActionJustPressed("a")){
			if(current.left!=null&&current.left!=nullNode){
				current.instance.set_visible(false);
				prev=current;
				current=current.left;
				current.instance.set_visible(true);
			}
		}
		if(Input.IsActionJustPressed("d")){
			if(current.right!=null&&current.right!=nullNode){
				current.instance.set_visible(false);
				prev=current;
				current=current.right;
				current.instance.set_visible(true);
			}
		}
		if(Input.IsActionJustPressed("w")){
			if(current!=null&&current!=nullNode) current.instance.set_visible(false);
			prev=null;
			current=root;
			current.instance.set_visible(true);
		}
	}

/// 
///         GENERAL
/// 

	double timer=-1;
	double time_duration=time_delay;

	bool delay(){
		if(timer!=-1){
			timer+=delta;
			if(timer>=time_duration) timer=-1;
			return true;
		}
		return false;
	}

	void start_timer(){
		timer=0;
	}

/// 
///         INSERT
/// 

	int insert_n;
	TreapNode insert_current=null;
	TreapNode insert_parent=null;
	bool inserting=false;

	int insert_phase=-1;

	private void insert_root(){
		root=new TreapNode(insert_n,nullNode,nullNode,random.Next()%10000);
		draw_node(ref root,ref nullNode);
		root.instance.Visible=true;
		root.instance.Position = new Vector2(0,0);
		Label label=root.instance.GetNode<Label>("Value");
		label.Text=$"{insert_n}";
		AddChild(root.instance);
		DisplayTreeStructure();
		inserting=false;
		insert_current=null;
		insert_parent=null;
	}

	private void insert_node(){
		TreapNode new_child=new TreapNode(insert_n,nullNode,nullNode,random.Next()%10000);
		draw_node(ref new_child,ref insert_parent);
		Label label=new_child.instance.GetNode<Label>("Value");
		label.Text=$"{insert_n}";

		new_child.instance.Visible=false;

		if(insert_n<insert_parent.element){
			insert_parent.left=new_child;
			
			new_child.instance.Position=new Vector2(
				-separation_x,
				separation_y
			);
		}
		else if(insert_n>insert_parent.element){
			insert_parent.right=new_child;
			
			new_child.instance.Position=new Vector2(
				separation_x,
				separation_y
			);
		}

		insert_phase=1;

		selector.set_move(new_child.instance.GlobalPosition);
		start_timer();

		insert_current=new_child;
	}

	Stack<(TreapNode,TreapNode,bool)> rotate_queue=new Stack<(TreapNode, TreapNode,bool)>();

	private void rotation_queue(){
		foreach((TreapNode,TreapNode,bool) item in rotate_queue){
			if(item.Item3){
				if(item.Item1.left.priority<item.Item1.priority){
					RotateWithLeftChild(item.Item1,item.Item2);
					timer=0.01;
				}
			}
			else{
				if(item.Item1.right.priority<item.Item1.priority){
					RotateWithRightChild(item.Item1,item.Item2);
					timer=0.01;
				}
			}
		}
		rotate_queue.Clear();
	}

	private void insert(){
		if(root==nullNode){
			insert_root();
			insert_phase=-1;
			inserting=false;
			insert_current=null;
			insert_parent=null;
			DisplayTreeStructure();
			reorder_tree(root);
			timer=-1;
			selector.set_move(new Vector2(0,0));
			return;
		}

		if(insert_current==nullNode){
			insert_node();
			return;
		}

		if(insert_n<insert_current.element){
			rotate_queue.Push((insert_current,insert_parent,true));

			insert_parent=insert_current;
			insert_current=insert_current.left;
		}
		else if(insert_n>insert_current.element){
			rotate_queue.Push((insert_current,insert_parent,false));

			insert_parent=insert_current;
			insert_current=insert_current.right;
		}

		if(insert_current!=null&&insert_current!=nullNode){
			selector.set_move(insert_current.instance.GlobalPosition);
			start_timer();
		}
	}

	public void begin_insert(int x){
		inserting=true;
		insert_n=x;
		insert_current=root;
		insert_parent=null;
		insert_phase=0;
		return;
	}

	void insert_phases(){
		if(delay()) return;
			
		if(insert_phase==0) insert();

		else if(insert_phase==1){
			reorder_tree(root);
			selector.set_move(new Vector2(0,0));
			insert_current.instance.Visible=true;
			insert_phase=2;
			start_timer();
		}

		else if(insert_phase==2){
			rotation_queue();
			DisplayTreeStructure();
			
			insert_phase=3;
			start_timer();
		}

		else if(insert_phase==3){
			reorder_tree(root);
			insert_phase=4;
			start_timer();
		}

		else if(insert_phase==4){
			reorder_tree(root);

			insert_current=null;
			insert_parent=null;
			inserting=false;
			insert_phase=-1;

			root.instance.set_move(new Vector2(0,0));
		}
	}

/// 
///         REMOVE
/// 

	int remove_n;
	TreapNode remove_current=null;
	TreapNode remove_parent=null;
	bool removing=false;

	int remove_phase=-1;

	void remove_find(){
		if(remove_current==nullNode||remove_current==null){ // not found
			GD.Print("not found");
			remove_phase=-1;
			remove_current=null;
			remove_parent=null;
			removing=false;
			return;
		}

		if(remove_n<remove_current.element){
			remove_parent=remove_current;
			remove_current=remove_current.left;

			if(remove_current!=null&&remove_current!=nullNode){
				selector.set_move(insert_current.instance.GlobalPosition);
				start_timer();
			}
		}
		else if(remove_n>remove_current.element){
			remove_parent=remove_current;
			remove_current=remove_current.right;

			if(remove_current!=null&&remove_current!=nullNode){
				selector.set_move(insert_current.instance.GlobalPosition);
				start_timer();
			}
		}
		else{ //found
			remove_phase=1;
			start_timer();
		}
	}

	void remove_rotate(){
		if(remove_current.left==nullNode||remove_current.right==nullNode){
			remove_phase=2;
			selector.set_move(remove_current.instance.GlobalPosition);
			start_timer();
			return;
		}

		if(remove_current.left.priority<remove_current.right.priority){
			TreapNode temp=remove_current.left;
			RotateWithLeftChild(remove_current,remove_parent);
			remove_parent=temp;
			//remove_current=remove_current.right;
		}
		else{
			TreapNode temp=remove_current.right;
			RotateWithRightChild(remove_current,remove_parent);
			remove_parent=temp;
			//remove_current=remove_current.left;
		}

		selector.set_move(remove_current.instance.GlobalPosition);
		start_timer();
	}

	void remove_node(){
		TreapNode oldNode=remove_current;

		if(remove_current.left==nullNode){
			if(remove_current==root){
				if(root.right!=nullNode){
					root=root.right;

					GDTreapNode child=remove_current.right.instance;
					RemoveChild(oldNode.instance);
					remove_current.instance.RemoveChild(child);
					AddChild(child);
				}
				else{
					root=nullNode;
					RemoveChild(oldNode.instance);   
				}
			}
			else{
				if(remove_parent.left==remove_current) remove_parent.left=remove_current.right;
				else remove_parent.right=remove_current.right;

				GDTreapNode child=remove_current.right.instance;
				remove_parent.instance.RemoveChild(remove_current.instance);
				remove_current.instance.RemoveChild(child);
				remove_parent.instance.AddChild(child);
			}
		}
		else{
			if(remove_current==root){
				if(root.left!=nullNode){
					root=root.left;

					GDTreapNode child=remove_current.left.instance;
					RemoveChild(oldNode.instance);
					remove_current.instance.RemoveChild(child);
					AddChild(child);
				}
				else{
					root=nullNode;
					RemoveChild(oldNode.instance);   
				}
			}
			else{
				if(remove_parent.left==remove_current) remove_parent.left=remove_current.left;
				else remove_parent.right=remove_current.left;

				GDTreapNode child=remove_current.left.instance;
				remove_parent.instance.RemoveChild(remove_current.instance);
				remove_current.instance.RemoveChild(child);
				remove_parent.instance.AddChild(child);
			}
		}

		oldNode.instance.QueueFree();

		selector.set_move(new Vector2(0,0));
		root.instance.set_move(new Vector2(0,0));
		remove_phase=3;
		reorder_tree(root);
	}

	public void begin_remove(int x){
		GD.Print("removing ",x);
		removing=true;
		remove_n=x;
		remove_current=root;
		remove_parent=null;
		remove_phase=0;
	}

	void remove_phases(){
		if(delay()) return;

		if(remove_phase==0){
			remove_find();
		}

		else if(remove_phase==1){
			remove_rotate();
		}

		else if(remove_phase==2){
			remove_node();
		}

		else if(remove_phase==3){
			reorder_tree(root);
			removing=false;
			remove_phase=-1;
			remove_current=null;
			remove_parent=null;
			DisplayTreeStructure();
		}
	}


/// 
///         PROCESS
/// 

	public override void _Process(double delta){
		this.delta=delta;

		wasd();

		if(inserting){
			insert_phases();
			return;
		}

		if(removing){
			remove_phases();
			return;
		}

		if(Input.IsActionJustPressed("ui_accept")){
			int x=random.Next()%1000;
			begin_insert(x);
		}

		if(Input.IsActionJustPressed("back")){
			int x=current.element;
			begin_remove(x);
		}

		if(Input.IsActionJustPressed("ui_cancel")){
			reorder_tree(root);
		}
	}
}
