package gameloop;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class EventUserDisconnect  extends BaseServerEventHandler
{
	@Override
	public void handleServerEvent(ISFSEvent event) throws SFSException 
	{
		// TODO Auto-generated method stub
		User sender = (User)event.getParameter(SFSEventParam.USER);
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();
		mainExt.GetGameInstance().RemoveTank(sender);
		trace("UserDisconnect - Remove user " + sender.getId());
		if(mainExt.getParentRoom().getPlayersList().size() <= 0)
		{
			mainExt.destroy();
			trace("Destroyed game");
		}
	}

}
