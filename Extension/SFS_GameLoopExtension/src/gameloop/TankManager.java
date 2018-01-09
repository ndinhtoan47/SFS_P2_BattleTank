package gameloop;

import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.mmo.Vec3D;

public class TankManager 
{
	protected RoomExtension _ext;
	protected Map<Integer, Tank> _tanks;
	public TankManager(RoomExtension ext)
	{
		_ext = ext;
		_tanks = new HashMap<Integer, Tank>();
	}
	public Map<Integer, Tank> GetTanks() {
		return _tanks;
	}
	public void RemoveTank(User user,Map<Integer,Boolean> readys) {
		if (user != null && _tanks.containsKey(user.getId())) {
			_tanks.remove(user.getId());
			if (readys.containsKey(user.getId()))
				readys.remove(user.getId());
		}
	}
	public void AddTank(User user) {
		_ext.trace("adding player");
		if (user != null && (!_tanks.containsKey(user.getId()))) {
			_tanks.put(user.getId(), new Tank(0, 0, 32, 32));
			_tanks.get(user.getId()).SetR(0);
			_ext.trace("added " + user.getName());
		} else {
			_ext.trace("can't add player");
		}
	}
	public void UpdateTanksState(float deltaTime) {
		for (Tank t : _tanks.values()) 
		{
			// check is playing
			if (t.IsActive())
			{
				// check state (alive or death)
				if (!t.IsAlive())
				{
					t.UpdateDeathDuration(deltaTime);
					if (t.GetDeathRemainDuration() <= 0) 
					{
						int key = GetKeyByValue(t);
						if (key != -1) {
							t.ReGeneration();
							User user = _ext.getParentRoom().getUserById(key);
							Vec3D regenerationPos = _ext.GetGameInstance().RadomPosition();
							UserVariable alive = new SFSUserVariable("alive", true);
							UserVariable x = new SFSUserVariable("x", (int) (regenerationPos.intX()));
							UserVariable y = new SFSUserVariable("y", (int) (regenerationPos.intY()));
							t.SetProperties((float)regenerationPos.intX(), (float)regenerationPos.intY());
							_ext.getApi().setUserVariables(user, Arrays.asList(alive, x, y), true, false);
							_ext.getMMOApi().setUserPosition(user,regenerationPos , _ext.getParentRoom());
							_ext.trace("Regeneration user with id is " + key);
						}
					}
				}
				t.HandleItem(deltaTime);
			}
		}
	}
	public void Update(float deltaTime)
	{
		this.UpdateTanksState(deltaTime);
	}
	public int GetKeyByValue(Tank value)
	{
		int key = -1;
		for(int b:_tanks.keySet())
		{
			if(_tanks.get(b) == value) 
			{
				key = b;
				break;
			}
		}
		return key;
	}

	public void IncreaseScore(int deadMan,int killer)
	{
		Tank deadManTank = (_tanks.containsKey(deadMan))?_tanks.get(deadMan):null;
		Tank killerTank = (_tanks.containsKey(killer))?_tanks.get(killer):null;
		if(deadManTank != null && killerTank != null)
		{
			deadManTank.Death(_ext, deadMan);
			killerTank.Kill(_ext, killer);
		}
	}
	
}
