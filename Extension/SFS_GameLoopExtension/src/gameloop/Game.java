package gameloop;

import java.util.HashMap;
import java.util.Map;

import com.smartfoxserver.v2.api.SFSMMOApi;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.ExceptionMessageComposer;
import com.smartfoxserver.v2.mmo.MMOItem;
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

	private Map<Integer, Boolean> _readys;
	private long _deltaTime;
	private long _lastTime;
	private int _primary;
	private SFSMMOApi _mmoApi;
	private Area[] _rdArea;

	// bullet test
	private BulletManager _bulletManager;
	// item test
	private ItemManager _itemsManager;
	// tank test
	private TankManager _tankManager;
	public Game(RoomExtension ext) {
		_ext = ext;		
		_readys = new HashMap<Integer, Boolean>();
		_deltaTime = 0;
		_lastTime = System.currentTimeMillis();
		_primary = -1;
		_bulletManager = new BulletManager(ext);
		_itemsManager = new ItemManager(ext);
		_tankManager = new TankManager(ext);
		_mmoApi = (SFSMMOApi)ext.getMMOApi();
		InitRdArea();
		_ext.trace("Game contrustor");
	}

	@Override
	public void run() {
		try {
			_deltaTime = System.currentTimeMillis() - _lastTime;
			_lastTime = System.currentTimeMillis();
			if (_ext.GetGameState() == RoomExtension.STATE_PLAYING) {
				float deltaTime = (float) _deltaTime / 1000.0f;
				_bulletManager.Update(deltaTime);
				_tankManager.Update(deltaTime);
				_itemsManager.Update(deltaTime);
			}
		} catch (Exception e) {
			// In case of exceptions this try-catch prevents the task to stop
			// running
			ExceptionMessageComposer emc = new ExceptionMessageComposer(e);
			_ext.trace(emc.toString());
		}
	}

	public Tank GetTankById(int id)
	{
		if(_tankManager.GetTanks().containsKey(id))
			return _tankManager.GetTanks().get(id);
		return null;
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

	public Vec3D RadomPosition() {
		_ext.trace("Randoming tank position");
		int area = RoomExtension.rd.nextInt(4);
		Vec3D startPos = _rdArea[area].start;
		Vec3D dimension = _rdArea[area].dimension;

		int x = (int) RoomExtension.rd.nextInt(startPos.intX() + dimension.intX());
		int y = (int) RoomExtension.rd.nextInt(startPos.intY() + dimension.intY());
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

	// bullet
	public void AddBullet(User sender) {
		_bulletManager.AddBullet(sender,_tankManager.GetTanks());
	}
	public void RemoveBullet(MMOItem bullet)
	{
		_bulletManager.Remove(bullet, _mmoApi);
	}
	// tank
	public Map<Integer,Tank> GetTanks()
	{
		return _tankManager.GetTanks();
	}
	public void RemoveTank(User user)
	{
		_tankManager.RemoveTank(user,_readys);
	}
	public void AddTank(User user)
	{
		_tankManager.AddTank(user);
	}
}
