using Godot;
using System;

public partial class LineDrawer : Control 
{
	public bool existe;
	public bool left;
	public bool right;
	public bool erase;
	public bool erase_l;
	Vector2 nodo_actual;
	Vector2 nodo_final;
	Vector2 s_right;
	Vector2 start;
	Vector2 end;
	Vector2 e_right;
	Vector2 e_left;
	Vector2 s_left;
	public override void _Ready()
	{
		existe=true;
	}

	public void draw_line(Vector2 A, Vector2 B, bool l){
		nodo_actual=new Vector2(A.X, A.Y );
		nodo_final= new Vector2(B.X, B.Y);
		if(l){
			left=true;
		}
		if(!l){
			right=true;
		}
	}

	public void erase_line(Vector2 A, Vector2 B, bool l){
		nodo_actual=new Vector2(A.X, A.Y );
		nodo_final= new Vector2(B.X, B.Y);
		if(l){
			left=true;
		}
		if(!l){
			right=true;
		}
		erase=true;
		
	}
public override void _Draw()
{
	base._Draw();



			float pi = (float)Math.PI;
			Vector2 centro_start = new Vector2(nodo_actual.X, nodo_actual.Y); 
			Vector2 centro_end =new Vector2(nodo_final.X,nodo_final.Y);

			float radio = 60.0f;
			float x_start;
			float y_start;
			float x_end;
			float y_end;
			if(right){
				x_start = (radio+3) * Mathf.Cos(45.0f*pi/180.0f) + centro_start.X;
				y_start = (radio+3) * Mathf.Sin(45.0f*pi/180.0f) + centro_start.Y;
				x_end = (radio-3) * Mathf.Cos(225.0f*pi/180.0f) + centro_end.X;
				y_end = (radio-3) * Mathf.Sin(225.0f*pi/180.0f) + centro_end.Y;
				start = new Vector2(x_start, y_start);
				end = new Vector2(x_end, y_end);
				if(erase){
					Color defaultBackgroundColor = new Color(76 / 255.0f, 76 / 255.0f, 76 / 255.0f, 255 / 255.0f);
					DrawLine(start, end, defaultBackgroundColor, 5.0f);
				}else{
					DrawLine(start, end, Colors.Black, 5.0f);
				}

			}
			if(left){
				x_start = (radio-3) * Mathf.Cos(135.0f*pi/180.0f) + centro_start.X;
				y_start = (radio-3) * Mathf.Sin(135.0f*pi/180.0f) + centro_start.Y;
				x_end = (radio+3)* Mathf.Cos(315.0f*pi/180.0f) + centro_end.X;
				y_end = (radio+3) * Mathf.Sin(315.0f*pi/180.0f) + centro_end.Y;
				start = new Vector2(x_start, y_start);
				end = new Vector2(x_end, y_end);
				if(erase){
					Color defaultBackgroundColor = new Color(76 / 255.0f, 76 / 255.0f, 76 / 255.0f, 255 / 255.0f);
					DrawLine(start, end, defaultBackgroundColor, 5.0f);
				}else{
					DrawLine(start, end, Colors.Black, 5.0f);
				}
			}
		
	
}

}
