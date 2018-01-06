package gameloop;

import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class RequestTest  extends BaseClientRequestHandler
{
	@Override
	public void handleClientRequest(User sender, ISFSObject params) 
	{
		trace(sender.getId() + "Test Request Handler Called");
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();				
		Game game = mainExt.GetGameInstance();				
		Map<Integer,Tank> tanks = game.GetTanks();
		if(tanks.containsKey(sender.getId()))
		{
			tanks.get(sender.getId()).Death(mainExt,sender.getId());
			trace("Set alive = false");
		}
		else trace("tanks doesn't contain " + sender.getId());
		
		trace("Test request process complete !");
	}

}