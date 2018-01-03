package gameloop;

public class Tank extends GameObject
{
	private double _rotation;
	private boolean _alive;
	
	public Tank(double x,double y,int w,int h)
	{
		super(x,y,w,h);
		super.SetType("tanktype");
		_alive = true;
	}
	
	public double GetR(){return _rotation;}
	public void SetR(double value) {_rotation = value;}
	public void SetProperties(double x, double y, double rotation)
	{
		this._rotation = rotation;
		super.SetProperties(x, y);
	}
	public boolean IsAlive(){return _alive;}
	public void ReGeneration(){_alive = true;}
	public void Death(){_alive = false;}
}
