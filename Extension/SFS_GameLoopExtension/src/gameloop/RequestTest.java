package gameloop;

import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class RequestTest extends BaseClientRequestHandler {
	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		trace(sender.getId() + "Test Request Handler Called");
		RoomExtension mainExt = (RoomExtension) this.getParentExtension();
		Game game = mainExt.GetGameInstance();
		Map<Integer, Tank> tanks = game.GetTanks();
		if (tanks.containsKey(sender.getId())) {
			Tank tank = tanks.get(sender.getId());
			if (tank.IsActive() && tank.IsAlive()) 
			{
				Item item = new Item(0,0,RoomExtension.ITEM_TYPE_ARMOR);
				tank.ReceiveItem(sender.getId(), item, mainExt);
			}
		}
		trace("Fire request process complete !");
	}

}