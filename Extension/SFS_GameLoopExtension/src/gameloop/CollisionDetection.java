package gameloop;

import java.awt.Rectangle;

public class  CollisionDetection 
{
	public CollisionDetection()
	{
		
	}
	
	public boolean AABB(Rectangle rect1, Rectangle rect2)
	{
		if (rect1.x <= rect2.x + rect2.width &&
				   rect1.x + rect1.width >= rect2.x &&
				   rect1.y <= rect2.y + rect2.height &&
				   rect1.height + rect1.y >= rect2.y) 
		{
			return true;
		}
		return false;
	}
}
