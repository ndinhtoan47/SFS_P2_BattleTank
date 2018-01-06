package gameloop;

public class GameObject 
{
	protected float _x;
	protected float _y;
	protected int _width;
	protected int _height;
	protected int _type;
	public GameObject(float x,float y,int w,int h)
	{
		_x = x; _y = y;
		_width = w; _height = h;
		_type = -1;
	}

	// properties
	public float GetX(){return _x;}
	public float GetY(){return _y;}
	public int GetWidth(){return _width;}
	public int GetHeight(){return _height;}
	public int GetType(){return _type;}
	public void SetType(int type){_type = type;}
	public void SetProperties(float x,float y)
	{
		_x = x; _y = y;
	}
	public void Update(float deltaTime) {
		
	}
}
