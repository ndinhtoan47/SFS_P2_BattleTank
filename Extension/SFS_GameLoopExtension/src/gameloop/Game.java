package gameloop;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.api.SFSMMOApi;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.exceptions.ExceptionMessageComposer;
import com.smartfoxserver.v2.mmo.BaseMMOItem;
import com.smartfoxserver.v2.mmo.IMMOItemVariable;
import com.smartfoxserver.v2.mmo.MMOItem;
import com.smartfoxserver.v2.mmo.MMOItemVariable;
import com.smartfoxserver.v2.mmo.MMORoom;
import com.smartfoxserver.v2.mmo.Vec3D;

public class Game implements Runnable {
	public class Area {
		public Area() {
			start = new Vec3D(0, 0, 0);
			dimension = new Vec3D(0, 0, 0);
		}

		public Vec3D start = new Vec3D(0, 0, 0);
		public Vec3D dimension = new Vec3D(0, 0, 0);
	}

	private RoomExtension _ext;
	private Map<Integer, Tank> _tanks;
	private Map<Integer, Boolean> _readys;
	private long _deltaTime;
	private long _lastTime;
	private int _primary;

	private Area[] _rdArea;

	// bullet test
	private BulletManager _bullets;

	public Game(RoomExtension ext) {
		_ext = ext;
		_tanks = new HashMap<Integer, Tank>();
		_readys = new HashMap<Integer, Boolean>();
		_deltaTime = 0;
		_lastTime = System.currentTimeMillis();
		_primary = -1;
		_bullets = new BulletManager(ext);
		InitRdArea();
		_ext.trace("Game contrustor");
	}

	@Override
	public void run() {
		try {
			_deltaTime = System.currentTimeMillis() - _lastTime;
			_lastTime = System.currentTimeMillis();
			if (_ext.GetGameState() == RoomExtension.STATE_PLAYING) {
				// _ext.trace("now :" + _deltaTime);
				// _ext.trace("user count :" +
				// _ext.getParentRoom().getUserList().size());
				// ISFSObject data = new SFSObject();
				// data.putLong("now", System.currentTimeMillis());
				// _ext.send("now", data, _ext.getParentRoom().getUserList());
				float deltaTime = (float) _deltaTime / 1000.0f;
				_bullets.Update(deltaTime);
				this.SaveBulletPositionToServer();
				this.UpdateTanks(deltaTime);
			}
		} catch (Exception e) {
			// In case of exceptions this try-catch prevents the task to stop
			// running
			ExceptionMessageComposer emc = new ExceptionMessageComposer(e);
			_ext.trace(emc.toString());
		}
	}

	public void SetTankProperties(User user, double x, double y, double rotation) {
		if (user != null && _tanks.containsKey(user.getId())) {
			_tanks.get(user.getId()).SetProperties(x, y, rotation);
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

	public Map<Integer, Tank> GetTanks() {
		return _tanks;
	}

	public void RemoveTank(User user) {
		if (user != null && _tanks.containsKey(user.getId())) {
			_tanks.remove(user.getId());
			if (_readys.containsKey(user.getId()))
				_readys.remove(user.getId());
		}
	}

	public long GetDeltaTime() {
		return _deltaTime;
	}

	public void Ready(User user) {
		if (_readys != null) {
			if (_readys.containsKey(user.getId())) {
				_readys.put(user.getId(), !(_readys.get(user.getId())));
				_ext.trace("User " + user.getId() + "ready is" + _readys.get(user.getId()));
			} else {
				_readys.put(user.getId(), true);
				_ext.trace("User " + user.getId() + "ready is true");
			}
		}
		_ext.trace("Ready list count : " + _readys.size());
	}

	public void InitRdArea() {
		try {
			_rdArea = new Area[4];
			for (int i = 0; i < 4; i++)
				_rdArea[i] = new Area();
			_rdArea[0].start = new Vec3D(0, 0, 0);
			_rdArea[0].dimension = new Vec3D(500, 500, 0);
			;
			_rdArea[1].start = new Vec3D(650, 0, 0);
			_rdArea[1].dimension = new Vec3D(320, 500, 0);
			_rdArea[2].start = new Vec3D(0, 550, 0);
			_rdArea[2].dimension = new Vec3D(500, 400, 0);
			_rdArea[3].start = new Vec3D(600, 900, 0);
			_rdArea[3].dimension = new Vec3D(360, 65, 0);
		} catch (Exception e) {
			_ext.trace(e.toString());
		}
	}

	public Vec3D RadomTankPosition() {
		_ext.trace("Randoming tank position");
		int area = _ext.rd.nextInt(4);
		Vec3D startPos = _rdArea[area].start;
		Vec3D dimension = _rdArea[area].dimension;

		int x = (int) _ext.rd.nextInt(startPos.intX() + dimension.intX());
		int y = (int) _ext.rd.nextInt(startPos.intY() + dimension.intY());
		Vec3D result = new Vec3D(x, y, 0);
		_ext.trace("Area = " + area);
		return result;
	}

	// properties
	public Map<Integer, Boolean> GetReadys() {
		return _readys;
	}

	public int GetPrimary() {
		return _primary;
	}

	public void SetPrimary(int primary) {
		_primary = primary;
	}

	// bullet test
	public void AddBullet(User sender) {
		_ext.trace("Adding bullet");
		Tank tankOfSender = null;
		if (_tanks.containsKey(sender.getId())) {
			tankOfSender = _tanks.get(sender.getId());
			_ext.trace("Got tank of sender");
		} else
			_ext.trace("Doesn't contain tank of sender ");
		if (tankOfSender != null) {
			double x = tankOfSender.GetX();
			double y = tankOfSender.GetY();
			double r = tankOfSender.GetR();
			if ((int) r == 0) {
				x += 20;
				y -= 5;
			}
			if ((int) r == 180) {
				x -= 20;
				y -= 5;
			}
			if ((int) r == -90) {
				x -= 5;
				y -= 20;
			}
			if ((int) r == 90) {
				x -= 5;
				y += 20;
			}
			_ext.trace("Set item variables");
			List<IMMOItemVariable> vars = new LinkedList<IMMOItemVariable>();
			vars.add(new MMOItemVariable("x", x));
			vars.add(new MMOItemVariable("y", y));
			vars.add(new MMOItemVariable("rotation", r));
			vars.add(new MMOItemVariable("type", "bullettype"));

			MMOItem bullet = new MMOItem(vars);
			SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
			MMORoom targetRoom = (MMORoom) _ext.getParentRoom();
			api.setMMOItemPosition(bullet, new Vec3D((int) x, (int) y, 0), targetRoom);
			_bullets.Add(bullet);
		} else
			_ext.trace("Tank of sender is null !");
	}

	public void SaveBulletPositionToServer() {
		// _ext.trace("Start save bullet variable and position !");
		SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
		MMORoom targetRoom = (MMORoom) _ext.getParentRoom();
		List<BaseMMOItem> items = targetRoom.getAllMMOItems();
		List<Integer> allItemId = new ArrayList<Integer>();
		for (BaseMMOItem it : items)
			allItemId.add(it.getId());

		for (int k : _bullets.GetBullets().keySet()) {
			if (allItemId.contains(k)) {
				MMOItem bullet = (MMOItem) targetRoom.getMMOItemById(k);
				List<IMMOItemVariable> vars = new LinkedList<IMMOItemVariable>();
				double x = _bullets.GetBullets().get(k).GetX();
				double y = _bullets.GetBullets().get(k).GetY();
				if (x < 0 || x > 1008 || y < 0 || y > 1008) {
					api.removeMMOItem(bullet);
					_ext.trace("Removed bullet with id is " + k);
				} else {
					vars.add(new MMOItemVariable("x", x));
					vars.add(new MMOItemVariable("y", y));

					api.setMMOItemVariables(bullet, vars, true);
					api.setMMOItemPosition(bullet, new Vec3D((int) x, (int) y, (int) 0), targetRoom);
					_ext.trace("Updated bullet with id " + k);
					_ext.trace("Bullet info : x = " + x + " y = " + y);
				}
			}
		}
		// _ext.trace("Save bullet variable and position complete !");
	}

	public void UpdateTanks(float deltaTime) {
		for (Tank t : _tanks.values()) {
			if (!t.IsAlive()) {
				_ext.trace("Had user death");
				t.UpdateDeathDuration(deltaTime);
				if (t.GetDeathRemainDuration() <= 0) {
					int key = GetTankKeyFromMap(_tanks, t);
					if (key != -1) {
						t.ReGeneration();
						User user = _ext.getParentRoom().getUserById(key);
						Vec3D regenerationPos = this.RadomTankPosition();
						UserVariable alive = new SFSUserVariable("alive", true);
						UserVariable x = new SFSUserVariable("x", (double)(regenerationPos.intX()));
						UserVariable y = new SFSUserVariable("y", (double)(regenerationPos.intY()));
						
						_ext.getApi().setUserVariables(user, Arrays.asList(alive,x,y), true,
								true);
						_ext.trace("Regeneration user with id is " + key);
					}
				}
			}
		}
	}

	// helper
	private int GetTankKeyFromMap(Map<Integer, Tank> map, Tank value) {
		int result = -1;
		for (int k : map.keySet()) {
			if (map.get(k) == value)
			{
				result = k;
				_ext.trace("found tank with user id is " + result);
				break;
			}
		}
		return result;
	}
}
