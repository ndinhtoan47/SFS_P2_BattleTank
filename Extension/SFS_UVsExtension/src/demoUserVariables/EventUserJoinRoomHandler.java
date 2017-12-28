package demoUserVariables;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class EventUserJoinRoomHandler extends BaseServerEventHandler
{

	@Override
	public void handleServerEvent(ISFSEvent event) throws SFSException 
	{
		// TODO Auto-generated method stub
		User sender = (User)event.getParameter(SFSEventParam.USER);
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();
		mainExt.GetGameInstance().AddTank(sender);
		
		ISFSObject outData = new SFSObject();
		if(mainExt.GetGameInstance().GetTanksCount() == 1)
		{
			trace(mainExt.GetGameInstance().GetTanksCount());
			outData.putBool("primary", true);	
			mainExt.GetGameInstance().Ready(sender);
			mainExt.send("isprimary", outData, sender);
		}
		else
		{
			outData.putBool("primary", false);
			mainExt.send("isprimary", outData, sender);
		}
		
		mainExt.GetGameInstance().SetPrimary(sender.getId());
		trace("Set primary success!");
	}

}
