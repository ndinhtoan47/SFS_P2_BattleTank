package gameloop;

import java.util.ArrayList;
import java.util.List;

import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.api.ISFSMMOApi;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import com.smartfoxserver.v2.mmo.MMORoom;
import com.smartfoxserver.v2.mmo.Vec3D;

public class EventUserJoinRoom extends BaseServerEventHandler
{

	@Override
	public void handleServerEvent(ISFSEvent event) throws SFSException 
	{
		trace("Join room event handler called !");
		try
		{
			User sender = (User)event.getParameter(SFSEventParam.USER);
			RoomExtension ext = (RoomExtension)this.getParentExtension();
			ISFSMMOApi api = SmartFoxServer.getInstance().getAPIManager().getMMOApi();
			
			MMORoom room = (MMORoom)ext.getParentRoom();
//			List<User> userJoinedRoom = room.getUserList();
//			List<Double> idJoined
//			for(User user:userJoinedRoom)
//			{
//				
//			}
			
			Game game = ext.GetGameInstance();
			game.AddTank(sender);
			
			UserVariable primary = new SFSUserVariable("primary",false);
			List<UserVariable> vars = new ArrayList<UserVariable>();
            vars.add((new SFSUserVariable("x", (double)0)));
            vars.add(new SFSUserVariable("y", (double)0));
            vars.add(new SFSUserVariable("rotation", (double)0));
			if(game.GetTanks().size() == 1)
			{				
				primary = new SFSUserVariable("primary",true);	
				game.SetPrimary(sender.getId());
				game.Ready(sender);
				trace(sender.getName() + "is primary");
			}
			else
			{
				trace(sender.getName() + "isn't primary");
			}
			vars.add(primary);
			//sender.setVariables(vars);
			ext.getApi().setUserVariables(sender, vars, true, true);
			api.setUserPosition(sender, new Vec3D(0,0,0), room);
			ISFSObject data = new SFSObject();
			data.putInt("roomonwer", game.GetPrimary());
			ext.send("isprimary", data, sender);
		}
		catch(Exception e)
		{
			trace(e.toString());
		}
	}

}
