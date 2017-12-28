package demoUserVariables;

import java.util.HashMap;
import java.util.Map;

import com.smartfoxserver.v2.entities.User;

public class Game 
{
	RoomExtension mainExt;
	Map<Integer,Tank> tanks;
	Map<Integer,Boolean> readys;
	int primary;
	public Game(RoomExtension main)
	{
		main = mainExt;
		tanks = new HashMap<Integer,Tank>();
		readys = new HashMap<Integer,Boolean>();
		primary = -1;
	}
	
	public void AddTank(User user)
	{
		if(tanks != null)
		{
			if(!tanks.containsKey(user.getId()))
			{
				tanks.put(user.getId(), new Tank(0,0));
				readys.put(user.getId(),false);
			}
		}

	}
	public void Ready(User user)
	{

		if(readys != null)
		{
			if(readys.containsKey(user.getId()))
			{
				readys.put(user.getId(), !(readys.get(user.getId())));
			}
			else
			{
				readys.put(user.getId(), true);
			}
		}
	}
	public void RemoveTank(User user)
	{
		if(tanks != null)
		{
			if(tanks.containsKey(user.getId()))
			{
				tanks.remove(user.getId());
			}
		}
	}
	
	public Map<Integer, Tank> GetTanks() {
		// TODO Auto-generated method stub
		if(tanks != null) return tanks;
		return null;
	}
	public Map<Integer,Boolean> GetUserReady()
	{
		return readys;
	}
	public int GetTanksCount()
	{
		if(tanks != null)
			return tanks.size();
		return -1;
	}
	public void SetPrimary(int key)
	{
		primary = key;
	}
	public int GetPrimary()	
	{
		return primary;
	}
}
