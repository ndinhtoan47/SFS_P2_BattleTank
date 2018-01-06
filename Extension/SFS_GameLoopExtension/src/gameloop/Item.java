package gameloop;

public class Item extends GameObject
{
	protected float _lifeTime;
	protected float _effectTime;
	protected  boolean _seftDestruct;
	
	public Item(float x, float y,int type) {
		super(x, y, 16, 16);
		switch(type)
		{
		case RoomExtension.ITEM_TYPE_ISVISIABLE:
			_effectTime = 5.0f;
			break;
		case RoomExtension.ITEM_TYPE_ARMOR:
			_effectTime = 10.0f;
			break;
		case RoomExtension.ITEM_TYPE_FREZZE:
			_effectTime = 3.0f;
			break;
		}
		_seftDestruct = false;
		_type = type;
		_lifeTime = ((float)RoomExtension.rd.nextInt(5000)/1000.0f) + 5.0f;
	}	
	public void Update(float deltaTime)
	{
		if(!_seftDestruct)
		{
			_lifeTime -= deltaTime;
			if(_lifeTime <= 0) _seftDestruct = true;
		}
	}

	public boolean IsSeftDestruct() {return _seftDestruct;}
}
