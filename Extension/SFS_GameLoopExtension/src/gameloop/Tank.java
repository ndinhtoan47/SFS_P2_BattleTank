package gameloop;

import java.util.ArrayList;
import java.util.List;

import com.smartfoxserver.v2.api.ISFSApi;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;

public class Tank extends GameObject {
	private double _rotation;
	private boolean _alive;
	private float _deathDuration;
	private boolean _isActive; // check user is playing or not

	public Tank(double x, double y, int w, int h) {
		super(x, y, w, h);
		super.SetType("tanktype");
		_alive = true;
		_deathDuration = 0;
		_isActive = false;
	}

	public double GetR() {
		return _rotation;
	}

	public void SetR(double value) {
		_rotation = value;
	}

	public void SetProperties(double x, double y, double rotation) {
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
			double curDeath = 0;
			if(user.containsVariable("death"))
			{
				curDeath = user.getVariable("death").getDoubleValue();
				curDeath++;
			}
			
			List<UserVariable> vars = new ArrayList<UserVariable>();
			vars.add(new SFSUserVariable("alive", false));
			vars.add(new SFSUserVariable("death",(double)curDeath));
			
			api.setUserVariables(user, vars, true, true);
			ext.trace("set death and alive variable");
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
}
