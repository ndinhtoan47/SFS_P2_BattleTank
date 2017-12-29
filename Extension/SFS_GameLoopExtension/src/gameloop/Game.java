package gameloop;

import java.util.HashMap;
import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.ExceptionMessageComposer;

public class Game implements Runnable
{
	private RoomExtension _ext;
	private Map<Integer,Tank> _tanks;
	
	public Game(RoomExtension ext)
	{
		_ext = ext;
		_tanks = new HashMap<Integer,Tank>();
	}
	@Override
	public void run()
	{
		try
		{
			//if(_ext.getParentRoom().getUserList().size() > 0)
			//{
				_ext.trace("now :" + System.currentTimeMillis());
				_ext.trace("user count :" + _ext.getParentRoom().getUserList().size());
				ISFSObject data = new SFSObject();
				data.putLong("now",  System.currentTimeMillis());
				_ext.send("now", data, _ext.getParentRoom().getUserList());
			//}
		}
		catch (Exception e)
		{
			// In case of exceptions this try-catch prevents the task to stop running
			ExceptionMessageComposer emc = new ExceptionMessageComposer(e);
			_ext.trace(emc.toString());
		}
	}
	
	public void AddTank(User user)
	{
		if(user != null && !_tanks.containsKey(user.getId()))
		{
			if(user.containsVariable("x") && user.containsVariable("y"))
			{
				_tanks.put(user.getId(), new Tank(user.getVariable("x").getDoubleValue(),user.getVariable("y").getDoubleValue()));
			}
		}
	}
	
}
