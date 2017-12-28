package demoUserVariables;

import java.util.ArrayList;
import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.mmo.MMORoom;

public class ReadyRequestHandler extends BaseClientRequestHandler
{

	@Override
	public void handleClientRequest(User sender, ISFSObject params) 
	{
		// TODO Auto-generated method stub
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();
		mainExt.GetGameInstance().Ready(sender);
		trace(sender.getId() + "ReadyRequestHandler");
		MMORoom room = (MMORoom)mainExt.getParentRoom();
		Boolean isPlayed = room.getVariable("isPlayed").getBoolValue();
						
		if(isPlayed)
		{
			List<UserVariable> vars = new ArrayList<UserVariable>();
			double x = mainExt.rd.nextDouble() * (double)1008;
			double y = mainExt.rd.nextDouble() * (double)1008;
			vars.add(new SFSUserVariable("x",x));
			vars.add(new SFSUserVariable("y",y));
			vars.add(new SFSUserVariable("rotation",(double)0));
			mainExt.getApi().setUserVariables(sender, vars, true, true);
			ISFSObject data = new SFSObject();
			data.putBool("canplay", true);
			mainExt.send("canplay", data, sender);		
		}
	}

}
