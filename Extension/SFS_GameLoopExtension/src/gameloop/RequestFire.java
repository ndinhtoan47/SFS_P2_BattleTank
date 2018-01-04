package gameloop;

import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class RequestFire extends BaseClientRequestHandler {
	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		trace(sender.getId() + "Fire Request Handler Called");
		RoomExtension mainExt = (RoomExtension) this.getParentExtension();
		Game game = mainExt.GetGameInstance();
		Map<Integer, Tank> tanks = game.GetTanks();
		if (tanks.containsKey(sender.getId())) {
			Tank tank = tanks.get(sender.getId());
			if (tank.IsActive() && tank.IsAlive()) {
				game.AddBullet(sender);
			}
		}
		trace("Fire request process complete !");
	}

}