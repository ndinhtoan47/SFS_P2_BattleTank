package gameloop;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.mmo.IMMOItemVariable;
import com.smartfoxserver.v2.mmo.MMOItem;

public class BulletManager 
{
	private RoomExtension _ext;
	private Map<Integer,GameObject> _bullets;
	
	public BulletManager(RoomExtension ext)
	{
		_ext = ext;
		_bullets = new HashMap<Integer,GameObject>();
	}
	
	public Map<Integer,GameObject> GetBullets()
	{
		return _bullets;
	}
	public void Add(MMOItem item)
	{
		if(item != null && !_bullets.containsKey(item.getId()))
		{
			List<IMMOItemVariable> vars = item.getVariables();
			List<String> varsName = new ArrayList<String>();
			for(IMMOItemVariable v:vars)
				varsName.add(v.getName());
			
			double x = 0;
			double y = 0;
			double w = 10;
			double h = 10;
			double r = 0;
			if(varsName.contains("x")) x = item.getVariable("x").getDoubleValue();
			if(varsName.contains("y")) y = item.getVariable("y").getDoubleValue();
			if(varsName.contains("rotation")) r = item.getVariable("rotation").getDoubleValue();
			_bullets.put(item.getId(), new Bullet(x,y,(int)w,(int)h));
			Bullet bullet = (Bullet)_bullets.get(item.getId());
			bullet.SetR(r);
			
			_ext.trace("Added bullet with id is " + item.getId() + " x = " + x + " y = " + y + " w = " + w + " h = " + h + " r = " + r);
		}
	}
	public void Remove(MMOItem item)
	{
		if(item != null && _bullets.containsKey(item.getId()))
		{
			_bullets.remove(item.getId());
			_ext.trace("Remove bullet with id " + item.getId());
		}
	}
	public void Update(float deltaTime)
	{
		for(GameObject obj:_bullets.values())
		{
			obj.Update(deltaTime);
		}
	}
}

