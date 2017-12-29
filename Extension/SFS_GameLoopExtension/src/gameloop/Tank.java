package gameloop;

public class Tank 
{
	private double posX;
	private double posY;
	private int width;
	private int height;
	public Tank(double x,double y)
	{
		posX = x;
		posY = y;
		width = 32;
		height = 32;
	}
	
	public double GetX(){return posX;}
	public double GetY(){return posY;}
	public int GetWidth(){return width;}
	public int GetHeight(){return height;}
}
