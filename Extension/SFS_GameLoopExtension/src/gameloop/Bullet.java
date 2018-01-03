package gameloop;

public class Bullet extends GameObject
{
	protected double rotation;
	public Bullet(double x, double y, int w, int h) 
	{
		super(x, y, w, h);
		super.SetType("bullettype");
		rotation = 0;
	}
	public double GetR(){return rotation;}
	public void SetR(double value) {rotation = value;}

	@Override
	public void Update(float deltaTime)
	{
		int yDir = (int) Math.sin(ConvertToRadian(rotation));
		int xDir = (int)Math.cos(ConvertToRadian(rotation));
		
		double newX = this._x  + xDir * (double)deltaTime * RoomExtension.SPEED_BULLET;
		double newY = this._y  + yDir * (double)deltaTime * RoomExtension.SPEED_BULLET;
		super.SetProperties(newX, newY);
		super.Update(deltaTime);
	}
	
	private double ConvertToRadian(double value)
	{
		double result = 0;
		// pi = 180
		result = (value * RoomExtension.PI)/180.0;
		return result;
	}
}
