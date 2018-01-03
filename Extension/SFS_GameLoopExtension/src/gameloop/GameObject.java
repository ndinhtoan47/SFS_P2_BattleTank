package gameloop;

public class GameObject 
{
	protected double _x;
	protected double _y;
	protected int _width;
	protected int _height;
	protected String _type;
	public GameObject(double x,double y,int w,int h)
	{
		_x = x; _y = y;
		_width = w; _height = h;
		_type = "";
	}

	// properties
	public double GetX(){return _x;}
	public double GetY(){return _y;}
	public int GetWidth(){return _width;}
	public int GetHeight(){return _height;}
	public String GetType(){return _type;}
	public void SetType(String type){_type = type;}
	public void SetProperties(double x,double y)
	{
		_x = x; _y = y;
	}
	public void Update(float deltaTime) {
		
	}
}
