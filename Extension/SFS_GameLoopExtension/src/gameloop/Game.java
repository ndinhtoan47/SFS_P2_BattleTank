package gameloop;

import java.util.HashMap;
import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.ExceptionMessageComposer;
import com.smartfoxserver.v2.mmo.Vec3D;

public class Game implements Runnable
{
	public class Area
	{
		public Area()
		{
			start = new Vec3D(0,0,0);
			dimension = new Vec3D(0,0,0);
		}
		public Vec3D start = new Vec3D(0,0,0);
		public Vec3D dimension = new Vec3D(0,0,0);
	}
	private RoomExtension _ext;
	private Map<Integer,Tank> _tanks;
	private Map<Integer,Boolean> _readys;
	private long _deltaTime;
	private long _lastTime;
	private int _primary;
	
	private Area[] _rdArea;
	public Game(RoomExtension ext)
	{
		_ext = ext;
		_tanks = new HashMap<Integer,Tank>();
		_readys = new HashMap<Integer,Boolean>();
		_deltaTime = 0;
		_lastTime = System.currentTimeMillis();
		_primary = -1;
		InitRdArea();
		_ext.trace("Game contrustor");
	}
	@Override
	public void run()
	{
		try
		{			
			_deltaTime = System.currentTimeMillis() - _lastTime;
			_lastTime  = System.currentTimeMillis();
			if(_ext.GetGameState() == RoomExtension.STATE_PLAYING)
			{
//				_ext.trace("now :" + System.currentTimeMillis());
//				_ext.trace("user count :" + _ext.getParentRoom().getUserList().size());
//				ISFSObject data = new SFSObject();
//				data.putLong("now",  System.currentTimeMillis());
//				_ext.send("now", data, _ext.getParentRoom().getUserList());
			}				
		}
		catch (Exception e)
		{
			// In case of exceptions this try-catch prevents the task to stop running
			ExceptionMessageComposer emc = new ExceptionMessageComposer(e);
			_ext.trace(emc.toString());
		}
	}
	
	public void SetTankProperties(User user,double x,double y,double rotation)
	{
		if(user != null && _tanks.containsKey(user.getId()))
		{
			_tanks.get(user.getId()).SetProperties(x,y,rotation);
		}
	}
	public void AddTank(User user)
	{
		_ext.trace("adding player");
		if(user != null && (!_tanks.containsKey(user.getId())))
		{
				_tanks.put(user.getId(), new Tank(0,0,0));
				_ext.trace("added " + user.getName());
		}
		else
		{
			_ext.trace("can't add player");
		}
	}
	public Map<Integer,Tank> GetTanks(){return _tanks;}

	public void RemoveTank(User user) 
	{
		if(user != null && _tanks.containsKey(user.getId()))
		{
			_tanks.remove(user.getId());
			if(_readys.containsKey(user.getId())) _readys.remove(user.getId());
		}
	}
	public long GetDeltaTime(){return _deltaTime;}
	public void Ready(User user) 
	{
		if(_readys != null)
		{
			if(_readys.containsKey(user.getId()))
			{
				_readys.put(user.getId(), !(_readys.get(user.getId())));
				_ext.trace("User " + user.getId() + "ready is" + _readys.get(user.getId()));
			}
			else
			{
				_readys.put(user.getId(), true);
				_ext.trace("User " + user.getId() + "ready is true");
			}
		}
		_ext.trace("Ready list count : " + _readys.size());
	}
	
	public void InitRdArea()
	{
		try
		{
		_rdArea = new Area[4];
		for(int i = 0; i < 4 ; i++)
			_rdArea[i] = new Area();
		_rdArea[0].start = new Vec3D(0,0,0);
		_rdArea[0].dimension = new Vec3D(500,500,0);;
		_rdArea[1].start = new Vec3D(650,0,0);
		_rdArea[1].dimension = new Vec3D(320,500,0);
		_rdArea[2].start = new Vec3D(0,550,0);
		_rdArea[2].dimension = new Vec3D(500,400,0);
		_rdArea[3].start = new Vec3D(600,900,0);
		_rdArea[3].dimension = new Vec3D(360,65,0);
		}
		catch(Exception e)
		{
			_ext.trace(e.toString());
		}
	}
	public Vec3D RadomTankPosition()
	{
		_ext.trace("Randoming tank position");
		int area = _ext.rd.nextInt(4);
		Vec3D startPos = _rdArea[area].start;
		Vec3D dimension = _rdArea[area].dimension;
		
		int x = (int)_ext.rd.nextInt(startPos.intX() + dimension.intX());
		int y = (int)_ext.rd.nextInt(startPos.intY() + dimension.intY());
		Vec3D result = new Vec3D(x,y,0);
		_ext.trace("Area = " + area);
		return result;
	}
	// properties
	public Map<Integer,Boolean> GetReadys()
	{
		return _readys;
	}
	public int GetPrimary(){return _primary;}
	public void SetPrimary(int primary){_primary = primary;}
}
