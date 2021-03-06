package gameloop;

import java.awt.Rectangle;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.api.SFSMMOApi;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.ExceptionMessageComposer;
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
	private float _roundTime;

	// bullet test
	private BulletManager _bulletManager;
	// item test
	private ItemManager _itemManager;
	// tank test
	private TankManager _tankManager;
	// collision test
	private CollisionDetection _collision;
	// Test
	MapManager _map;
	QuadTree _quadTree;

	public Game(RoomExtension ext) {
		_ext = ext;
		_readys = new HashMap<Integer, Boolean>();
		_deltaTime = 0;
		_lastTime = System.currentTimeMillis();
		_primary = -1;
		_bulletManager = new BulletManager(ext);
		_itemManager = new ItemManager(ext);
		_tankManager = new TankManager(ext);
		_mmoApi = (SFSMMOApi) ext.getMMOApi();
		_collision = new CollisionDetection();
		_roundTime = 0;
		if (_ext.getParentRoom().containsVariable("time")) {
			_roundTime = (float) _ext.getParentRoom().getVariable("time").getIntValue();
			_ext.trace("round time = " + _roundTime);
		} else {
			_ext.trace("don't contain variable time");
		}
		InitRdArea();

		// test
		_map = new MapManager(_ext);
		_map.inl();
		_quadTree = new QuadTree(0, new Rectangle(0, 0, 1008, 1008));
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
				_itemManager.Update(deltaTime);

				this.CheckCollision();
				this.UpdateRoomState(deltaTime);
				// this.CheckCollisionWithQuadTree();
			}
		} catch (Exception e) {
			// In case of exceptions this try-catch prevents the task to stop
			// running
			ExceptionMessageComposer emc = new ExceptionMessageComposer(e);
			_ext.trace(emc.toString());
		}
	}

	public Tank GetTankById(int id) {
		if (_tankManager.GetTanks().containsKey(id))
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
			_rdArea = new Area[3];
			for (int i = 0; i < 3; i++)
				_rdArea[i] = new Area();
			_rdArea[0].start = new Vec3D(48, 48, 0);
			_rdArea[0].dimension = new Vec3D(452, 452, 0);
			_rdArea[1].start = new Vec3D(700, 48, 0);
			_rdArea[1].dimension = new Vec3D(260, 452, 0);
			_rdArea[2].start = new Vec3D(48, 650, 0);
			_rdArea[2].dimension = new Vec3D(452, 310, 0);
			// _rdArea[3].start = new Vec3D(640, 896, 0);
			// _rdArea[3].dimension = new Vec3D(624, 454, 0);
		} catch (Exception e) {
			_ext.trace(e.toString());
		}
	}

	public Vec3D RadomPosition() {
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
		_bulletManager.AddBullet(sender, _tankManager.GetTanks());
	}

	public void RemoveBullet(Bullet bullet) {
		_bulletManager.Remove(bullet, _mmoApi);
	}

	// tank
	public Map<Integer, Tank> GetTanks() {
		return _tankManager.GetTanks();
	}

	public void RemoveTank(User user) {
		_tankManager.RemoveTank(user, _readys);
	}

	public void AddTank(User user) {
		_tankManager.AddTank(user);
	}

	public void CheckCollision() {
		this.CheckBulletVsUser();
		this.CheckItemVsUser();
		this.CheckBulletVsBullet();
	}

	protected void CheckItemVsUser() {
		Map<Integer, Tank> tanks = _tankManager.GetTanks();
		Map<Integer, Item> items = _itemManager.GetItems();
		Rectangle tank = new Rectangle();
		Rectangle item = new Rectangle();
		for (Tank t : tanks.values()) {
			if (t.IsAlive()) {
				for (Item i : items.values()) {
					// get tank properties
					tank.x = (int) t.GetX() - 16;
					tank.y = (int) t.GetY() - 16;
					tank.width = (int) t.GetWidth();
					tank.height = (int) t.GetHeight();
					// get item properties
					item.x = (int) i.GetX();
					item.y = (int) i.GetY();
					item.width = (int) i.GetWidth();
					item.height = (int) i.GetHeight();
					boolean result = _collision.AABB(tank, item);
					if (result) {
						_itemManager.Remove(i, _mmoApi);
						t.ReceiveItem(_tankManager.GetKeyByValue(t), i, _ext);
					}
				}
			}
		}
	}

	protected void CheckBulletVsUser() {
		Map<Integer, Tank> tanks = _tankManager.GetTanks();
		Map<Integer, Bullet> bullets = _bulletManager.GetBullets();
		Rectangle tank = new Rectangle();
		Rectangle bullet = new Rectangle();
		for (Tank t : tanks.values()) {
			if (t.IsAlive())
				for (Bullet b : bullets.values()) {
					if (b.GetOnwer() != _tankManager.GetKeyByValue(t)) {
						// get tank properties
						tank.x = (int) t.GetX() - 16;
						tank.y = (int) t.GetY() - 16;
						tank.width = (int) t.GetWidth();
						tank.height = (int) t.GetHeight();
						// get bullet properties
						bullet.x = (int) b.GetX();
						bullet.y = (int) b.GetY();
						bullet.width = (int) b.GetWidth();
						bullet.height = (int) b.GetHeight();
						boolean result = _collision.AABB(tank, bullet);
						if (result) {
							// tank swap to death state and remove bullet
							_ext.trace("Check");
							if (t.GetHodingItem() != RoomExtension.ITEM_TYPE_ARMOR) {
								_ext.trace("Don't hoding armor , hoding item is " + t.GetHodingItem());
								_tankManager.IncreaseScore(_tankManager.GetKeyByValue(t), b.GetOnwer());
							} else {
								_ext.trace("Reset hoding item");
								t.ResetHodingItem(_ext, _tankManager.GetKeyByValue(t));
							}
							_bulletManager.Remove(b, _mmoApi);

						}
					}
				}
		}
	}

	protected void CheckBulletVsBullet() {
		Map<Integer, Bullet> bullets1 = _bulletManager.GetBullets();
		Map<Integer, Bullet> bullets2 = _bulletManager.GetBullets();

		Rectangle bullet1 = new Rectangle();
		Rectangle bullet2 = new Rectangle();
		for (Bullet b1 : bullets1.values()) {
			for (Bullet b2 : bullets2.values()) {
				if (b2 != b1) {
					// get bullet 1 properties
					bullet1.x = (int) b1.GetX();
					bullet1.y = (int) b1.GetY();
					bullet1.width = (int) b1.GetWidth();
					bullet1.height = (int) b1.GetHeight();
					// get bullet 2 properties
					bullet2.x = (int) b2.GetX();
					bullet2.y = (int) b2.GetY();
					bullet2.width = (int) b2.GetWidth();
					bullet2.height = (int) b2.GetHeight();
					boolean result = _collision.AABB(bullet1, bullet2);
					if (result) {
						_bulletManager.Remove(b1, _mmoApi);
						_bulletManager.Remove(b2, _mmoApi);
					}
				}
			}
		}
	}

	protected void UpdateRoomState(float deltaTime) {
		if (_roundTime <= 0 && _ext.GetGameState() != RoomExtension.STATE_WAIT) {
			_roundTime = 0;
			_ext.SetGameState(RoomExtension.STATE_WAIT);
			_ext.trace("set room vars");
			ISFSObject data = new SFSObject();
			data.putBool("value", false);
			_ext.send("gameisplaying", data, _ext.getParentRoom().getUserList());
			return;
		} else
			_roundTime -= deltaTime;
	}

	public boolean CheckCollisionTankWithTitle(GameObject tank) 
	{
		List<CollisionTitle> titles = _map.GetLayout().getCollisionListTiles();
		for (GameObject t : titles)
		{
			Rectangle tRect = new Rectangle();
			Rectangle tankRect = new Rectangle();

			// get title properties
			tRect.x = (int) t.GetX();
			tRect.y = (int) t.GetY();
			tRect.width = (int) t.GetWidth();
			tRect.height = (int) t.GetHeight();
			// get tank properties
			tankRect.x = (int) tank.GetX();
			tankRect.y = (int) tank.GetY();
			tankRect.width = (int) tank.GetWidth();
			tankRect.height = (int) tank.GetHeight();
			if (_collision.AABB(tRect, tankRect)) 
			{
				_ext.trace("collision");
				return true;

			}			
		}
		return false;
	}

	public boolean CheckCollisionWithQuadTree(GameObject tank) {
		_quadTree.clear();

		// Map<Integer,Tank> tanks = _tankManager.GetTanks();
		List<CollisionTitle> titles = _map.GetLayout().getCollisionListTiles();

		List<GameObject> all = new ArrayList<GameObject>();
		// for(GameObject tank:tanks.values())
		// all.add((GameObject)tank);
		all.add(tank);
		for (GameObject title : titles)
			all.add((GameObject) title);

		List<GameObject> collisionObj = new ArrayList<GameObject>();

		for (GameObject obj : all)
			_quadTree.insert(obj);
		// for(GameObject obj:all)
		// {
		collisionObj.clear();
		// _quadTree.retrieve(collisionObj, obj);
		_quadTree.retrieve(collisionObj, tank);
		for (GameObject obj1 : collisionObj) {
			if (obj1 != tank) {
				for (GameObject obj2 : collisionObj) {
					if (obj2.GetType() == RoomExtension.ES_TILE) {
						Rectangle obj1Rect = new Rectangle();
						Rectangle obj2Rect = new Rectangle();

						// get tank properties
						obj1Rect.x = (int) obj1.GetX();
						obj1Rect.y = (int) obj1.GetY();
						obj1Rect.width = (int) obj1.GetWidth();
						obj1Rect.height = (int) obj1.GetHeight();
						// get title properties
						obj2Rect.x = (int) obj2.GetX();
						obj2Rect.y = (int) obj2.GetY();
						obj2Rect.width = (int) obj2.GetWidth();
						obj2Rect.height = (int) obj2.GetHeight();
						if (_collision.AABB(obj1Rect, obj2Rect)) {
							_ext.trace("title collsion with tank " + obj2.GetX() + " " + obj2.GetY());
							_ext.trace("Collision object count " + collisionObj.size());
							return true;

						}

					}
				}
			}
		}
		return false;
		// }
	}
}
