package demoUserVariables;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSRoomVariable;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class PlayRequestHandler extends BaseClientRequestHandler
{

	@Override
	public void handleClientRequest(User sender, ISFSObject params) 
	{
		// TODO Auto-generated method stub
		trace("PlayRequestHandler is requested !");
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();
		Game game = mainExt.GetGameInstance();
		Map<Integer,Tank> tanks = game.GetTanks();
		
		ISFSObject outData = new SFSObject();
		boolean canPlay = false;
		trace("Loopping tanks key");
		List<User> receives = new ArrayList<User>();
		if(tanks != null)
		{
			try
			{
				Map<Integer,Boolean> readys = game.GetUserReady();
				Collection<Integer> idCanPlay = new ArrayList<Integer>();
				trace("Getting user can play ...");
				for(int k:readys.keySet())
					if(readys.get(k) == true && tanks.containsKey(k))
					{
						idCanPlay.add((int)k);
						User recipient = mainExt.getParentRoom().getUserById(k);
						if(recipient != null) receives.add(recipient);
					}
				if(idCanPlay.size() <= 1) 
				{
					outData.putUtfString("message","there are not player readyed !");
				}
				else
				{
					canPlay = true;
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
						}
					}
					trace("set room variable isPlayed !");
					mainExt.getParentRoom().setVariable(new SFSRoomVariable("isPlayed",true));
				}
			}
			catch(Exception e)
			{
				outData.putUtfString("error", e.toString());
			}
		}
		outData.putBool("canplay", canPlay);
		mainExt.send("canplay", outData, receives);
	}
	

}
