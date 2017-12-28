package demoUserVariables;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import com.smartfoxserver.v2.mmo.MMORoom;

public class EventUserLeaveRoom extends BaseServerEventHandler
{

	@Override
	public void handleServerEvent(ISFSEvent event) throws SFSException 
	{
		// TODO Auto-generated method stub
		User sender = (User)event.getParameter(SFSEventParam.USER);
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();
		mainExt.GetGameInstance().RemoveTank(sender);
		MMORoom room = (MMORoom)mainExt.getParentRoom();
		if(room.getPlayersList().size() <= 0)
		{
			room.setVariable(new SFSRoomVariable("isPlayed",false));
			mainExt.destroy();
		}
	}

}
