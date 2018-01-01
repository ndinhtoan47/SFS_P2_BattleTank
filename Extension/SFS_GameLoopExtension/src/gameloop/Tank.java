package gameloop;

public class Tank 
{
	private double posX;
	private double posY;
	private double rotation;
	private int width;
	private int height;
	public Tank(double x,double y,double rotation)
	{
		posX = x;
		posY = y;
		this.rotation = rotation;
		width = 32;
		height = 32;
	}
	
	public double GetX(){return posX;}
	public double GetY(){return posY;}
	public double GetR(){return rotation;}
	public int GetWidth(){return width;}
	public int GetHeight(){return height;}

	public void SetProperties(double x, double y, double rotation)
	{
		posX = x;
		posY = y;
		this.rotation = rotation;
	}
}
