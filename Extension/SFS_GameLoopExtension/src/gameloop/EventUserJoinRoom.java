package gameloop;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

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
			// add tank
			Game game = ext.GetGameInstance();
			game.AddTank(sender);
			
			UserVariable primary = new SFSUserVariable("primary",false);
			List<UserVariable> vars = new ArrayList<UserVariable>();
            vars.add((new SFSUserVariable("x", (int)0)));
            vars.add(new SFSUserVariable("y", (int)0));
            vars.add(new SFSUserVariable("rotation", (int)0));
//            vars.add(new SFSUserVariable("alive",(boolean)true));
//            vars.add(new SFSUserVariable("kill",(int)0));
//            vars.add(new SFSUserVariable("death",(int)0));
            // get users readied
            List<Short> id = new ArrayList<Short>();
            List<Boolean> isReady = new ArrayList<Boolean>();
            Map<Integer,Boolean> readys = game.GetReadys();
            for(int k:readys.keySet())
            {
            	if(readys.containsKey(k))
            	{
            		id.add((short)k);
            		isReady.add(readys.get(k));
            	}
            }          
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
			ext.getApi().setUserVariables(sender, vars, true, false);
			api.setUserPosition(sender, new Vec3D(0,0,0), room);
			ISFSObject data = new SFSObject();
			data.putShort("roomonwer", (short)game.GetPrimary());
			data.putShortArray("idarray", id);
			data.putBoolArray("readyarray", isReady);
			ext.send("isprimary", data, sender);
		}
		catch(Exception e)
		{
			trace(e.toString());
		}
	}

}
