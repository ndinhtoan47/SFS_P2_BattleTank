package gameloop;

import java.util.Arrays;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import com.smartfoxserver.v2.mmo.MMORoom;

public class EventUserDisconnect  extends BaseServerEventHandler
{
	@Override
	public void handleServerEvent(ISFSEvent event) throws SFSException 
	{
		// TODO Auto-generated method stub
		User sender = (User)event.getParameter(SFSEventParam.USER);
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();
		mainExt.GetGameInstance().RemoveTank(sender);
		MMORoom room = (MMORoom)mainExt.getParentRoom();
		trace("UserDisconnect - Remove user " + sender.getId());
		if(mainExt.getParentRoom().getUserList().size() <= 0)
		{
			RoomVariable state = new SFSRoomVariable("state",(int)RoomExtension.STATE_WAIT);
			room.setVariables(Arrays.asList(state));
			mainExt.SetGameState((int)RoomExtension.STATE_WAIT);
			trace("Destroyed game");
			trace("room state " + room.getVariable("state"));
			mainExt.destroy();	
		}
	}

}
