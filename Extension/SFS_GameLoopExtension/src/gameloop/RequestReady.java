package gameloop;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.mmo.MMORoom;
import com.smartfoxserver.v2.mmo.Vec3D;

public class RequestReady extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		trace(sender.getId() + "ReadyRequestHandler");
		RoomExtension mainExt = (RoomExtension) this.getParentExtension();
		Game game = mainExt.GetGameInstance();
		game.Ready(sender);
		Map<Integer, Tank> tanks = game.GetTanks();

		MMORoom room = (MMORoom) mainExt.getParentRoom();
		trace("Game state now : " + room.getVariable("state"));
		if (room.getVariable("state").getIntValue() == RoomExtension.STATE_PLAYING) {
			if (tanks.containsKey(sender.getId()))
			{
				tanks.get(sender.getId()).Active();
				trace("user " + sender.getId() + " active");
				Vec3D position = game.RadomPosition();
				List<UserVariable> vars = new ArrayList<UserVariable>();
				int x = position.intX();
				int y = position.intY();

				vars.add(new SFSUserVariable("x", (int)x));
				vars.add(new SFSUserVariable("y", (int)y));
				vars.add(new SFSUserVariable("rotation", (int) 0));
				mainExt.getApi().setUserVariables(sender, vars, true, true);

				ISFSObject outData = new SFSObject();
				outData.putBool("canplay", true);
				mainExt.send("canplay", outData, sender);
			}
		}
	}

}
