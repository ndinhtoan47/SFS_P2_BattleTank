package demoUserVariables;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Map;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class GetAllIdHandler extends BaseClientRequestHandler
{

	@Override
	public void handleClientRequest(User sender, ISFSObject params) 
	{
		// TODO Auto-generated method stub
		RoomExtension mainExt = (RoomExtension)this.getParentExtension();
		Collection<Double> id = new ArrayList<Double>();
		Map<Integer,Tank> tanks = mainExt.GetGameInstance().GetTanks();
		ISFSObject outData = new SFSObject();
		if(tanks != null)
		{
			for(int i:tanks.keySet())
			{
				id.add((double)i);
			}
		}
		outData.putDoubleArray("id",id);
		mainExt.send("allid", outData, mainExt.getParentRoom().getUserList());
	}
	
}
