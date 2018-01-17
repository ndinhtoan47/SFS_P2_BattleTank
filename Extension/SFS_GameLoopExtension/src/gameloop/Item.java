package gameloop;

public class Item extends GameObject
{
	protected float _lifeTime;
	protected float _effectTime;
	protected  boolean _seftDestruct;
	protected boolean _isFireCountDownRequest;
	public Item(float x, float y,int type) {
		super(x, y, 16, 16);
		switch(type)
		{
		case RoomExtension.ITEM_TYPE_ISVISIABLE:
			_effectTime = 5.0f;
			break;
		case RoomExtension.ITEM_TYPE_ARMOR:
			_effectTime = 20.0f;
			break;
		case RoomExtension.ITEM_TYPE_FREZZE:
			_effectTime = 5.0f;
			break;
		}
		_seftDestruct = false;
		_type = type;
		_lifeTime = ((float)RoomExtension.rd.nextInt(3000)/1000.0f) + 10.0f;
		_isFireCountDownRequest = false;
	}	
	public void Update(float deltaTime)
	{
		if(!_seftDestruct)
		{			
			if(_lifeTime <= 0) _seftDestruct = true;
		}
		_lifeTime -= deltaTime;
	}
	public boolean IsSeftDestruct() {return _seftDestruct;}
	public boolean IsFireCountDown(){return _isFireCountDownRequest;}
	public void FireCountDown()
	{
		_isFireCountDownRequest = true;
	}
	public float GetLifeTime()
	{
		return _lifeTime;
	}
	public float GetEffectTime()
	{
		return _effectTime;
	}
}
