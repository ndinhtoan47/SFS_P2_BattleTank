package gameloop;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.RoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.mmo.MMORoom;

public class RequestPlay extends BaseClientRequestHandler
{

	@Override
	public void handleClientRequest(User sender, ISFSObject params) 
	{
		trace("Play request called by" + sender.getName());
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();				
		Game game = mainExt.GetGameInstance();
		Map<Integer,Tank> tanks = game.GetTanks();
		List<User> receives = new ArrayList<User>();
		boolean canPlay = false;
		
		MMORoom room = (MMORoom)mainExt.getParentRoom();
		ISFSObject outData = new SFSObject();
		if(mainExt.GetGameInstance().GetReadys().size() >= 2)
		{
			mainExt.SetGameState((int)RoomExtension.STATE_PLAYING);
			RoomVariable state = new SFSRoomVariable("state",(int)RoomExtension.STATE_PLAYING);
			room.setVariables(Arrays.asList(state));
			trace("set room variable state playing !");			
			canPlay = true;
		}
		else
		{
			trace("Put message !");
			outData.putUtfString("message","there are not player readyed !");
		}
		Map<Integer,Boolean> readys = game.GetReadys();
		Collection<Integer> idCanPlay = new ArrayList<Integer>();
		trace("Getting user can play ...");
		for(int k:readys.keySet())
			if(readys.get(k) == true && tanks.containsKey(k))
			{
				idCanPlay.add((int)k);
				User recipient = mainExt.getParentRoom().getUserById(k);
				if(recipient != null) receives.add(recipient);
			}
		trace("IdCanPlay size = " + idCanPlay.size());
		
		if(canPlay)
		{
			for(int k:idCanPlay)
			{
				if(tanks.containsKey(k))
				{
					trace(k);						
					User player = mainExt.getParentRoom().getUserById(k);
					List<UserVariable> vars = new ArrayList<UserVariable>();
					double x = mainExt.rd.nextDouble() * (double)1008;
					double y = mainExt.rd.nextDouble() * (double)1008;
					vars.add(new SFSUserVariable("x",x));
					vars.add(new SFSUserVariable("y",y));
					vars.add(new SFSUserVariable("rotation",(double)0));
					mainExt.getApi().setUserVariables(player, vars, true, true);
					trace("set user variable " + k);
				}
			}				
		}
		outData.putBool("canplay", canPlay);
		mainExt.send("canplay", outData, receives);
		trace("Game state now : " + room.getVariable("state"));	
		///////////////////////
	}
}
