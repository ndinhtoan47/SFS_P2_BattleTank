package gameloop;

public class Bullet extends GameObject
{
	protected int _rotation;
	protected int _onwer;
	public Bullet(float x, float y,int rotation,int onwer) 
	{
		super(x, y, 10, 10);
		super.SetType(RoomExtension.ITEM_TYPE_BULLET);
		_rotation = rotation;
		_onwer = onwer;
	}
	public int GetR(){return _rotation;}
	public void SetR(int value) {_rotation = value;}
	@Override
	public void Update(float deltaTime)
	{
		int yDir = (int) Math.sin(RoomExtension.ConvertToRadian(_rotation));
		int xDir = (int)Math.cos(RoomExtension.ConvertToRadian(_rotation));
		
		float newX = this._x  + (float)xDir * (float)deltaTime * (float)RoomExtension.SPEED_BULLET;
		float newY = this._y  + (float)yDir * (float)deltaTime * (float)RoomExtension.SPEED_BULLET;
		super.SetProperties(newX, newY);
		super.Update(deltaTime);
	}
	public int GetOnwer(){return _onwer;}
}
