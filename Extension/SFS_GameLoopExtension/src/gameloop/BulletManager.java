package gameloop;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.api.SFSMMOApi;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.mmo.BaseMMOItem;
import com.smartfoxserver.v2.mmo.IMMOItemVariable;
import com.smartfoxserver.v2.mmo.MMOItem;
import com.smartfoxserver.v2.mmo.MMOItemVariable;
import com.smartfoxserver.v2.mmo.MMORoom;
import com.smartfoxserver.v2.mmo.Vec3D;

public class BulletManager {
	private RoomExtension _ext;
	private Map<Integer, GameObject> _bullets;

	public BulletManager(RoomExtension ext) {
		_ext = ext;
		_bullets = new HashMap<Integer, GameObject>();
	}

	public Map<Integer, GameObject> GetBullets() {
		return _bullets;
	}

	protected void Add(MMOItem item, int onwer , int rotation) {
		if (item != null && !_bullets.containsKey(item.getId())) {
			List<IMMOItemVariable> vars = item.getVariables();
			List<String> varsName = new ArrayList<String>();
			for (IMMOItemVariable v : vars)
				varsName.add(v.getName());

			float x = 0;
			float y = 0;
			if (varsName.contains("x"))
				x = (float) item.getVariable("x").getIntValue();
			if (varsName.contains("y"))
				y = (float) item.getVariable("y").getIntValue();
			_bullets.put(item.getId(), new Bullet((float) x, (float) y, rotation, onwer));
			_ext.trace("Added bullet with id is " + item.getId() + " x = " + x + " y = " + y + " w = 10" +" h = 10"
					+ " r = " + rotation);
		}
	}

	public void Remove(MMOItem item, SFSMMOApi api) {
		if (item != null && _bullets.containsKey(item.getId())) {
			_bullets.remove(item.getId());
			api.removeMMOItem(item);
			_ext.trace("Remove bullet with id " + item.getId());
		}
	}

	public void Update(float deltaTime) {
		for (GameObject obj : _bullets.values()) {
			obj.Update(deltaTime);
		}
		SaveBulletPositionToServer();
	}

	public void SaveBulletPositionToServer() {
		// _ext.trace("Start save bullet variable and position !");
		SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
		MMORoom targetRoom = (MMORoom) _ext.getParentRoom();
		List<BaseMMOItem> items = targetRoom.getAllMMOItems();
		List<Integer> allItemId = new ArrayList<Integer>();
		for (BaseMMOItem it : items)
			allItemId.add(it.getId());

		for (int k : _bullets.keySet()) {
			if (allItemId.contains(k)) {
				MMOItem bullet = (MMOItem) targetRoom.getMMOItemById(k);
				List<IMMOItemVariable> vars = new LinkedList<IMMOItemVariable>();
				Bullet b = (Bullet) _bullets.get(k);
				float x = b.GetX();
				float y = b.GetY();
				int r = (int) b.GetR();
				if (x <= 0 || x >= 1008 || y <= 0 || y >= 1008) {
					this.Remove(bullet, api);
					_ext.trace("Removed bullet with id is " + k);
				} else {
					if (r == 0 || r == 180)
						vars.add(new MMOItemVariable("x", (int) x));
					else
						vars.add(new MMOItemVariable("y", (int) y));

					api.setMMOItemVariables(bullet, vars, true);
					api.setMMOItemPosition(bullet, new Vec3D((int) x, (int) y, (int) 0), targetRoom);
					_ext.trace("Updated bullet with id " + k);
					_ext.trace("Bullet info : x = " + x + " y = " + y);
				}
			}
		}
		// _ext.trace("Save bullet variable and position complete !");
	}

	public void AddBullet(User sender, Map<Integer, Tank> tanks) {
		_ext.trace("Adding bullet");
		Tank tankOfSender = null;
		if (tanks.containsKey(sender.getId())) {
			tankOfSender = tanks.get(sender.getId());
			_ext.trace("Got tank of sender");
		} else
			_ext.trace("Doesn't contain tank of sender ");
		if (tankOfSender != null) {
			float x = tankOfSender.GetX();
			float y = tankOfSender.GetY();
			int r = tankOfSender.GetR();
			if ((int) r == 0) {
				x += 20;
				y -= 5;
			}
			if ((int) r == 180) {
				x -= 20;
				y -= 5;
			}
			if ((int) r == -90) {
				y -= 20;
				x -= 5;
			}
			if ((int) r == 90) {
				y += 20;
				x -= 5;
			}
			_ext.trace("Set item variables");
			List<IMMOItemVariable> vars = new LinkedList<IMMOItemVariable>();
			vars.add(new MMOItemVariable("x", (int) x));
			vars.add(new MMOItemVariable("y", (int) y));
			vars.add(new MMOItemVariable("type", RoomExtension.ITEM_TYPE_BULLET));
			// vars.add(new MMOItemVariable("onwer", (int) sender.getId()));

			MMOItem bullet = new MMOItem(vars);
			SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
			MMORoom targetRoom = (MMORoom) _ext.getParentRoom();
			api.setMMOItemPosition(bullet, new Vec3D((int) x, (int) y, 0), targetRoom);
			this.Add(bullet, sender.getId(),r);
		} else
			_ext.trace("Tank of sender is null !");
	}
}
