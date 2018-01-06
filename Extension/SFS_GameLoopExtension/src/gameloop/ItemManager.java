package gameloop;

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

public class ItemManager 
{
	protected Map<Integer,Item> _items;
	protected float _delaySpawItem;
	protected float _totalSpawItem;
	protected RoomExtension _ext;
	public ItemManager(RoomExtension ext)
	{
		_items = new HashMap<Integer,Item>();
		_delaySpawItem = 3;
		_totalSpawItem = 0;
		_ext = ext;
	}
	
	public void Update(float deltaTime)
	{
		for(Item it:_items.values())
		{
			it.Update(deltaTime);
		}
		this.AddItem(deltaTime);
		this.RemoveItem();
	}
	
	public Map<Integer,Item> GetItems()
	{
		return _items;
	}
	
	protected void AddItem(float deltaTime)
	{
		if((_delaySpawItem <= _totalSpawItem) == true)
		{
			_ext.trace("enter add item method");
			Game game = _ext.GetGameInstance();
			Vec3D rdPos = game.RadomPosition();
			
			int itemType = RoomExtension.rd.nextInt(3);
			int type = -1;
			switch(itemType)
			{
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
						
			Item i = new Item((int)rdPos.intX(),(int)rdPos.intY(),type);
			List<IMMOItemVariable> vars = new LinkedList<IMMOItemVariable>();
			vars.add(new MMOItemVariable("x",(int)i.GetX()));
			vars.add(new MMOItemVariable("y",(int)i.GetY()));
			vars.add(new MMOItemVariable("type",(int)type));
			
			MMOItem item = new MMOItem(vars);
			SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
			MMORoom targetRoom = (MMORoom) _ext.getParentRoom();
			api.setMMOItemPosition(item, rdPos, targetRoom);	
			_items.put(item.getId(), i);
			_delaySpawItem = 5 + (RoomExtension.rd.nextInt(5000)/ 1000.0f);
			
			_ext.trace("Added item with id " + "X = " + i.GetX() + " Y = " + i.GetY());
			
			_totalSpawItem = 0;
		} else {
			_totalSpawItem += deltaTime;
			}
	}

	protected void RemoveItem()
	{
		for(Item it:_items.values())
		{
			if(it.IsSeftDestruct())
			{
				//_ext.trace(it.IsSeftDestruct());
				SFSMMOApi api = (SFSMMOApi) _ext.getMMOApi();
				MMORoom targetRoom = (MMORoom) _ext.getParentRoom();
				BaseMMOItem item = null;
				int id = this.GetItemId(_items, it);
				if(id != -1)
				{
					if(targetRoom.containsMMOItem(id))
					{
						item = targetRoom.getMMOItemById(id);
						api.removeMMOItem(item);
						_items.remove(id);
						//_ext.trace("Removed item with id " + id);
					}
					
				}				
			}
		}
	}
	
	private int GetItemId(Map<Integer,Item> map, Item value)
	{
		int result = -1;
		for (int k : map.keySet()) {
			if (map.get(k) == value) {
				result = k;
				break;
			}
		}
		return result;
	}
}
