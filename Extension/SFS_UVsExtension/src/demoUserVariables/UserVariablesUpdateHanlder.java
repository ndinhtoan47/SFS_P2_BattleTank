package demoUserVariables;

import com.smartfoxserver.v2.api.ISFSMMOApi;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import com.smartfoxserver.v2.mmo.Vec3D;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.SmartFoxServer;

public class UserVariablesUpdateHanlder extends BaseServerEventHandler
{

	@Override
	public void handleServerEvent(ISFSEvent params) throws SFSException 
	{
		// TODO Auto-generated method stub
		ISFSMMOApi mmoApi = SmartFoxServer.getInstance().getAPIManager().getMMOApi();
		User sender = (User)params.getParameter(SFSEventParam.USER);
		RoomExtension ext = (RoomExtension)this.getParentExtension();
		
		 @SuppressWarnings("unchecked")
			List<UserVariable> variables = (List<UserVariable>) params.getParameter(SFSEventParam.VARIABLES);
			  Map<String,UserVariable> varMap = new HashMap<String, UserVariable>();
		        for (UserVariable var : variables)
		        {
		            varMap.put(var.getName(), var);
		        }
		         
		        if (varMap.containsKey("x") || varMap.containsKey("y"))
		        {
		            Vec3D pos = new Vec3D
		            (
		                varMap.get("x").getDoubleValue().intValue(),
		                varMap.get("y").getDoubleValue().intValue(),
		                1
		            );
		             trace(pos);
		            mmoApi.setUserPosition(sender, pos, ext.getParentRoom());
		        }	
	}

}
