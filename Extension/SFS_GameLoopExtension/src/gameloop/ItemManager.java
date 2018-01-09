package gameloop;

import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.api.SFSMMOApi;
import com.smartfoxserver.v2.mmo.BaseMMOItem;
import com.smartfoxserver.v2.mmo.IMMOItemVariable;
import com.smartfoxserver.v2.mmo.MMOItem;
import com.smartfoxserver.v2.mmo.MMOItemVariable;
import com.smartfoxserver.v2.mmo.MMORoom;
import com.smartfoxserver.v2.mmo.Vec3D;

public class ItemManager {
	protected Map<Integer, Item> _items;
	protected float _delaySpawItem;
	protected float _totalSpawItem;
	protected RoomExtension _ext;

	public ItemManager(RoomExtension ext) {
		_items = new HashMap<Integer, Item>();
		_delaySpawItem = 3;
		_totalSpawItem = 0;
		_ext = ext;
	}

	public void Update(float deltaTime) {
		
		this.AddItem(deltaTime);
		this.RemoveItem();
		for (Item it : _items.values()) {
			it.Update(deltaTime);
			this.UpdateItemState(it);
		}
	}

	public Map<Integer, Item> GetItems() {
		return _items;
	}

	protected void AddItem(float deltaTime) {
		if ((_delaySpawItem <= _totalSpawItem) == true) {
			Game game = _ext.GetGameInstance();
			Vec3D rdPos = game.RadomPosition();

			int itemType = RoomExtension.rd.nextInt(3);
			int type = -1;
			switch (itemType) {
			case 0:
				type = RoomExtension.ITEM_TYPE_ARMOR;
				break;
			case 1:
				type = RoomExtension.ITEM_TYPE_FREZZE;
				break;
			case 2:
				type = RoomExtension.ITEM_TYPE_ISVISIABLE;
				break;
			}

			Item i = new Item((int) rdPos.intX(), (int) rdPos.intY(), type);
			List<IMMOItemVariable> vars = new LinkedList<IMMOItemVariable>();
			vars.add(new MMOItemVariable("x", (int) i.GetX()));
			vars.add(new MMOItemVariable("y", (int) i.GetY()));
			vars.add(new MMOItemVariable("type", (int) type));

			MMOItem item = new MMOItem(vars);
			SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
			MMORoom targetRoom = (MMORoom) _ext.getParentRoom();
			api.setMMOItemPosition(item, rdPos, targetRoom);
			_items.put(item.getId(), i);
			_delaySpawItem = 5 + (RoomExtension.rd.nextInt(5000) / 1000.0f);

			_totalSpawItem = 0;
		} else {
			_totalSpawItem += deltaTime;
		}
	}

	private void RemoveItem() 
	{
		for (Item it : _items.values()) {
			if (it.IsSeftDestruct()) {
				SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
				MMORoom targetRoom = (MMORoom) _ext.getParentRoom();
				BaseMMOItem item = null;
				int id = this.GetKeyByValue(it);
				if (id != -1) {
					if (targetRoom.containsMMOItem(id)) {
						item = targetRoom.getMMOItemById(id);
						api.removeMMOItem(item);
						_items.remove(id);
					}
				}
			}
		}
	}

	public void Remove(Item item, SFSMMOApi api) {
		MMORoom room = (MMORoom) _ext.getParentRoom();
		int itemId = GetKeyByValue(item);
		if (itemId != -1 && room.containsMMOItem(itemId)) {
			MMOItem it = (MMOItem) room.getMMOItemById(itemId);
			if (it != null) {
				_items.remove(itemId);
				api.removeMMOItem(it);
				_ext.trace("Remove bounus with id " + itemId);
			}
		}
	}
	protected void UpdateItemState(Item item) {
		if (!item.IsFireCountDown() && item.GetLifeTime() <= 3) 
		{
			MMORoom room = (MMORoom) _ext.getParentRoom();
			SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
			int itId = this.GetKeyByValue(item);
			MMOItem mmoIt = null;
			if(room.containsMMOItem(itId))
			{
				mmoIt = (MMOItem) room.getMMOItemById(itId);
			}
			if (mmoIt != null)
			{
				MMOItemVariable countdown = new MMOItemVariable("countdown", true); 
				api.setMMOItemVariables(mmoIt, Arrays.asList(countdown), true);
				item.FireCountDown();
				_ext.trace("Count down item " + itId);
			}
			else _ext.trace("mmoIt = null");
		}
	}

	public int GetKeyByValue(Item value) {
		int key = -1;
		for (int b : _items.keySet()) {
			if (_items.get(b) == value) {
				key = b;
				break;
			}
		}
		return key;
	}
}
