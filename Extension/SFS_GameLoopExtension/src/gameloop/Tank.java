package gameloop;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import com.smartfoxserver.v2.api.ISFSApi;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;

public class Tank extends GameObject {
	private int _rotation;
	private boolean _alive;
	private float _deathDuration;
	private boolean _isActive; // check user is playing or not
	private int _death;
	private int _kill;
	// handle item
	private int _hodingItem;
	private float _itemEffectTime;
	
	public Tank(float x, float y, int w, int h) {
		super(x, y, w, h);
		super.SetType(RoomExtension.ES_TANK);
		_alive = true;
		_deathDuration = 0;
		_isActive = false;
		_kill = 0;
		_death = 0;
		// handle item
		_hodingItem = -1;
		_itemEffectTime = 0;
	}

	public int GetR() {
		return _rotation;
	}

	public void SetR(int value) {
		_rotation = value;
	}

	public void SetProperties(float x, float y, int rotation) {
		this._rotation = rotation;
		super.SetProperties(x, y);
	}

	public boolean IsAlive() {
		return _alive;
	}

	public void ReGeneration() {
		_alive = true;
	}

	public void Death(RoomExtension ext, int id) {
		_deathDuration = RoomExtension.DEATH_DURATION;
		_alive = false;
		ISFSApi api = ext.getApi();
		if (api.getUserById(id) != null)
		{
			User user = api.getUserById(id);
			_death++;			
			List<UserVariable> vars = new ArrayList<UserVariable>();
			vars.add(new SFSUserVariable("alive", _alive));
			vars.add(new SFSUserVariable("death",(int)_death));
			
			api.setUserVariables(user, vars, true, false);
		}
		else
			ext.trace("user can't found !");
	}

	public void Kill(RoomExtension ext,int myId)
	{
		_kill++;
		ISFSApi api = ext.getApi();
		if (api.getUserById(myId) != null)
		{
			User user = api.getUserById(myId);			
			List<UserVariable> vars = new ArrayList<UserVariable>();
			vars.add(new SFSUserVariable("kill",(int)_kill));			
			api.setUserVariables(user, vars, true, false);
		}
		else
			ext.trace("user can't found !");
	}
	public void UpdateDeathDuration(float deltaTime) {
		if (!_alive)
			_deathDuration -= deltaTime;
	}

	public float GetDeathRemainDuration() {
		return _deathDuration;
	}
	
	public void Active(){_isActive = true;}
	
	public void DeActive(){_isActive = false;}
	
	public boolean IsActive(){return _isActive;}
	
	public void X_AxisMove(int rotation,float deltaTime)
	{
		if(rotation == _rotation)
		{			
			int dir = (int)Math.cos(RoomExtension.ConvertToRadian(rotation));
			_x += dir * deltaTime * RoomExtension.SPEED_TANK;
		}
		else _rotation = rotation;
	}
	public void Y_AxisMove(int rotation,float deltaTime)
	{
		if(rotation == _rotation)
		{
			int dir = (int) Math.sin(RoomExtension.ConvertToRadian(rotation));
			_y += dir * deltaTime * RoomExtension.SPEED_TANK;
		}
		else _rotation = rotation;
	}
	public void ReceiveItem(int receiveUser,Item item,RoomExtension ext)
	{
		_hodingItem = item.GetType();
		_itemEffectTime = item.GetEffectTime();
		UserVariable receiceItem = new SFSUserVariable("item",_hodingItem);
		User user = ext.getParentRoom().getUserById(receiveUser);
		ext.getApi().setUserVariables(user, Arrays.asList(receiceItem),true, false);
	}
	public void HandleItem(float deltaTime)
	{
		if(_hodingItem != -1)
		{
			if(_itemEffectTime <= 0)
			{
				_hodingItem = -1;
				_itemEffectTime = 0;
				return;
			}
			_itemEffectTime -= deltaTime;
		}
	}
	public int GetHodingItem()
	{
		return _hodingItem;
	}
	public void ResetHodingItem(RoomExtension ext,int receiveUser)
	{
		_hodingItem = -1;
		_itemEffectTime = 0.0f;
		UserVariable receiceItem = new SFSUserVariable("item",_hodingItem);
		User user = ext.getParentRoom().getUserById(receiveUser);
		ext.getApi().setUserVariables(user, Arrays.asList(receiceItem),true, false);
		ext.trace("reset hoding item success");
	}
}
