package gameloop;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.api.ISFSMMOApi;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import com.smartfoxserver.v2.mmo.Vec3D;

public class EventUserVariableUpdate extends BaseServerEventHandler {

	@Override
	public void handleServerEvent(ISFSEvent params) throws SFSException {
		ISFSMMOApi mmoApi = SmartFoxServer.getInstance().getAPIManager().getMMOApi();
		User sender = (User) params.getParameter(SFSEventParam.USER);
		RoomExtension ext = (RoomExtension) this.getParentExtension();
		Game game = ext.GetGameInstance();

		if (game.GetTanks().containsKey(sender.getId())) 
		{
			if (game.GetTanks().get(sender.getId()).IsAlive()) 
			{
				@SuppressWarnings("unchecked")
				List<UserVariable> variables = (List<UserVariable>) params.getParameter(SFSEventParam.VARIABLES);
				Map<String, UserVariable> varMap = new HashMap<String, UserVariable>();
				for (UserVariable var : variables)
					varMap.put(var.getName(), var);

				if (varMap.containsKey("x") && varMap.containsKey("y") && varMap.containsKey("rotation")) {
					Vec3D pos = new Vec3D(varMap.get("x").getDoubleValue().intValue(),
							varMap.get("y").getDoubleValue().intValue(), 0);
					mmoApi.setUserPosition(sender, pos, ext.getParentRoom());
					game.SetTankProperties(sender, varMap.get("x").getDoubleValue(), varMap.get("y").getDoubleValue(),
							varMap.get("rotation").getDoubleValue());
					trace(sender.getName() + "user variable update " + "Position :" + pos);
				}
			}
		}

	}
}
