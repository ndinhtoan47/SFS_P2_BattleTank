package gameloop;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.api.ISFSMMOApi;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import com.smartfoxserver.v2.mmo.Vec3D;

public class EventUserVariableUpdate extends BaseServerEventHandler {

	@Override
	public void handleServerEvent(ISFSEvent params) throws SFSException
	{

		RoomExtension ext = (RoomExtension) this.getParentExtension();
		ISFSMMOApi mmoApi = SmartFoxServer.getInstance().getAPIManager().getMMOApi();
		User sender = (User) params.getParameter(SFSEventParam.USER);
		Game game = ext.GetGameInstance();
		if (ext.GetGameState() == RoomExtension.STATE_PLAYING) 
		{
			if (game.GetTanks().containsKey(sender.getId())) 
			{
				if (game.GetTanks().get(sender.getId()).IsAlive()) 
				{
					// get variables and store into map
					@SuppressWarnings("unchecked")
					List<UserVariable> variables = (List<UserVariable>) params.getParameter(SFSEventParam.VARIABLES);
					Map<String, UserVariable> varMap = new HashMap<String, UserVariable>();
					for (UserVariable var : variables)
						varMap.put(var.getName(), var);

					Tank tankOfSender = game.GetTankById(sender.getId());
					if (tankOfSender != null) 
					{
						if (varMap.containsKey("rotation")) {
							float deltaTime = (float) (game.GetDeltaTime() / 1000.0f);
							int rotation = varMap.get("rotation").getIntValue();
							List<UserVariable> newVars = new ArrayList<UserVariable>();
							int x = (int) (tankOfSender.GetX());
							int y = (int) (tankOfSender.GetY());
							if (rotation == 0 || rotation == 180)
							{
								tankOfSender.X_AxisMove(rotation, deltaTime,true);
								x = (int) (tankOfSender.GetX());
								newVars.add(new SFSUserVariable("x", (int) x));
							} else
							{
								tankOfSender.Y_AxisMove(rotation, deltaTime,true);
								y = (int) (tankOfSender.GetY());
								newVars.add(new SFSUserVariable("y", (int) y));
							}
//							if(game.CheckCollisionWithQuadTree(tankOfSender))
//							{
//								if(rotation == 0 || rotation == 180)
//								{
//									if(rotation == 0) 
//										tankOfSender.X_AxisMove(180, deltaTime, true);
//									else 
//										tankOfSender.X_AxisMove(0, deltaTime, true);
//									x = (int) (tankOfSender.GetX());
//									newVars.add(new SFSUserVariable("x", (int) x));
//									ext.trace("X collsion");
//								}
//								else
//								{
//									if(rotation == 90) 
//										tankOfSender.Y_AxisMove(-90, deltaTime, true);
//									else 
//										tankOfSender.Y_AxisMove(90, deltaTime, true);
//									y = (int) (tankOfSender.GetY());
//									newVars.add(new SFSUserVariable("y", (int) y));
//									ext.trace("Y collsion");
//								}
//							}
							if(game.CheckCollisionTankWithTitle(tankOfSender))
							{
								if (rotation == 0 || rotation == 180)
								{
									if(rotation == 0)
									tankOfSender.X_AxisMove(180, deltaTime,false);
									else tankOfSender.X_AxisMove(0, deltaTime,false);
									x = (int) (tankOfSender.GetX());
									newVars.add(new SFSUserVariable("x", (int) x));
								} else
								{
									if(rotation == 90)
									tankOfSender.Y_AxisMove(-90, deltaTime,false);
									else tankOfSender.Y_AxisMove(90, deltaTime,false);
									y = (int) (tankOfSender.GetY());
									newVars.add(new SFSUserVariable("y", (int) y));
								}
							}
							ext.getApi().setUserVariables(sender, newVars, true, false);
							mmoApi.setUserPosition(sender, new Vec3D((int) x, (int) y, 0), ext.getParentRoom());
						}
					}
					
				}
			}

		}
	}
}
