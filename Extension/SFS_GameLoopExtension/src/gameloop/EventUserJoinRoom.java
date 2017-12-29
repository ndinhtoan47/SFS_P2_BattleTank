package gameloop;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class EventUserJoinRoom extends BaseServerEventHandler
{

	@Override
	public void handleServerEvent(ISFSEvent event) throws SFSException 
	{
		trace("Join room event handler called !");
		// TODO Auto-generated method stub
		try
		{
			User sender = (User)event.getParameter(SFSEventParam.USER);
			RoomExtension ext = (RoomExtension)this.getParentExtension();
			ext.GetGameInstance().AddTank(sender);
			trace("added user: " + sender);
		}
		catch(Exception e)
		{
			trace(e.toString());
		}
	}

}
